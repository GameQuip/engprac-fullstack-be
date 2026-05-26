using Backend.Data;
using Backend.DTOs.JobApplication;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[ApiController]
[Route("api/v1/job-applications")]
public class JobApplicationsController : ControllerBase
{
    private readonly AppDbContext _context;

    public JobApplicationsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> ApplyJob([FromBody] ApplyJobRequest request)
    {
        var jobExists = await _context.Jobs.AnyAsync(j => j.Id == request.JobId);
        var userExists = await _context.Users.AnyAsync(u => u.Id == request.UserId);

        if (!jobExists)
        {
            return BadRequest("Job does not exist");
        }

        if (!userExists)
        {
            return BadRequest("User does not exist");
        }

        var alreadyApplied = await _context.JobApplications
            .AnyAsync(a => a.JobId == request.JobId && a.UserId == request.UserId);

        if (alreadyApplied)
        {
            return BadRequest("User already applied to this job");
        }

        var application = new JobApplication
        {
            JobId = request.JobId,
            UserId = request.UserId,
            Status = "Applied",
            AppliedAt = DateTime.UtcNow
        };

        _context.JobApplications.Add(application);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            application.Id,
            application.JobId,
            application.UserId,
            application.Status,
            application.AppliedAt
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetApplications()
    {
        var applications = await _context.JobApplications
            .Include(a => a.Job)
            .Include(a => a.User)
            .Select(a => new
            {
                a.Id,
                a.Status,
                a.AppliedAt,
                Job = new
                {
                    a.Job!.Id,
                    a.Job.Title,
                    a.Job.CompanyName
                },
                User = new
                {
                    a.User!.Id,
                    a.User.FullName,
                    a.User.Email
                }
            })
            .ToListAsync();

        return Ok(applications);
    }

    [HttpGet("user/{userId:int}")]
    public async Task<IActionResult> GetApplicationsByUser(int userId)
    {
        var applications = await _context.JobApplications
            .Include(a => a.Job)
            .Where(a => a.UserId == userId)
            .Select(a => new
            {
                a.Id,
                a.Status,
                a.AppliedAt,
                Job = new
                {
                    a.Job!.Id,
                    a.Job.Title,
                    a.Job.CompanyName,
                    a.Job.Location
                }
            })
            .ToListAsync();

        return Ok(applications);
    }
}