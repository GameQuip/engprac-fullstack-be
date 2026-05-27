using System.Text;
using Backend.Data;
using Backend.Models;
using Backend.Repositories;
using Backend.Services;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var allowedOrigins = builder.Configuration["AllowedOrigins"]?
    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
    ?? ["http://localhost:4200"];

builder.Services.AddCors(options =>
    options.AddPolicy("AllowAngularFrontend",
        policy => policy.WithOrigins(allowedOrigins)
                        .AllowAnyMethod()
                        .AllowAnyHeader()));

var jwtSecret = builder.Configuration["JwtSettings:Secret"]
    ?? throw new InvalidOperationException("JwtSettings:Secret not configured.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (!string.IsNullOrEmpty(connectionString))
    builder.Services.AddDbContext<AppDbContext>(o => o.UseNpgsql(connectionString));
else
    builder.Services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase("JobTrackDb"));

builder.Services.AddScoped<JobRepository>();
builder.Services.AddScoped<JobService>();
builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (db.Database.IsInMemory())
        db.Database.EnsureCreated();
    else
        db.Database.Migrate();

    SeedData(db);
}

app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();

if (!app.Environment.IsProduction())
    app.UseHttpsRedirection();
app.UseCors("AllowAngularFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

static void SeedData(AppDbContext db)
{
    if (db.Users.Any()) return;

    var admin = new User
    {
        FullName = "Alice Admin", Email = "alice@example.com", Role = "Admin",
        PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
        CreatedAt = DateTime.UtcNow
    };
    var user1 = new User
    {
        FullName = "Bob User", Email = "bob@example.com", Role = "User",
        PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
        CreatedAt = DateTime.UtcNow
    };
    var user2 = new User
    {
        FullName = "Carol User", Email = "carol@example.com", Role = "User",
        PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
        CreatedAt = DateTime.UtcNow
    };
    db.Users.AddRange(admin, user1, user2);
    db.SaveChanges();

    var job1 = new Job
    {
        Title = "Software Engineer", CompanyName = "TechCorp",
        Description = "Build scalable web applications.", Location = "Bangkok",
        Status = "Open", Type = "Full-time", RelatedUserId = admin.Id,
        CreatedAt = DateTime.UtcNow.AddDays(-10)
    };
    var job2 = new Job
    {
        Title = "UX Designer", CompanyName = "CreativeHub",
        Description = "Design beautiful user experiences.", Location = "Remote",
        Status = "Open", Type = "Contract", RelatedUserId = admin.Id,
        CreatedAt = DateTime.UtcNow.AddDays(-5)
    };
    var job3 = new Job
    {
        Title = "Data Analyst", CompanyName = "DataFlow",
        Description = "Analyze business data and insights.", Location = "Chiang Mai",
        Status = "Closed", Type = "Full-time", RelatedUserId = admin.Id,
        CreatedAt = DateTime.UtcNow.AddDays(-20)
    };
    db.Jobs.AddRange(job1, job2, job3);
    db.SaveChanges();

    db.JobApplications.AddRange(
        new JobApplication { JobId = job1.Id, UserId = user1.Id, Status = "Applied", AppliedAt = DateTime.UtcNow.AddDays(-3) },
        new JobApplication { JobId = job2.Id, UserId = user1.Id, Status = "Passed", AppliedAt = DateTime.UtcNow.AddDays(-7) },
        new JobApplication { JobId = job1.Id, UserId = user2.Id, Status = "Applied", AppliedAt = DateTime.UtcNow.AddDays(-1) }
    );
    db.SaveChanges();
}
