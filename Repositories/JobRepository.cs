using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class JobRepository
{
    private readonly AppDbContext _dbContext;

    public JobRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Job>> GetAllAsync()
    {
        return await _dbContext.Jobs
            .OrderByDescending(j => j.CreatedAt)
            .ToListAsync();
    }

    public async Task<Job?> GetByIdAsync(int id)
    {
        return await _dbContext.Jobs
            .FirstOrDefaultAsync(j => j.Id == id);
    }

    public async Task<Job?> GetByIdForUpdateAsync(int id)
    {
        return await _dbContext.Jobs
            .FirstOrDefaultAsync(j => j.Id == id);
    }

    public async Task AddAsync(Job job)
    {
        await _dbContext.Jobs.AddAsync(job);
    }

    public void Delete(Job job)
    {
        _dbContext.Jobs.Remove(job);
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}