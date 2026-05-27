using Backend.DTOs.Job;
using Backend.Models;
using Backend.Repositories;

namespace Backend.Services;

public class JobService
{
    private readonly JobRepository _jobRepository;

    public JobService(JobRepository jobRepository)
    {
        _jobRepository = jobRepository;
    }

    public async Task<List<JobResponse>> GetAllJobsAsync()
    {
        var jobs = await _jobRepository.GetAllAsync();
        return jobs.Select(ToResponse).ToList();
    }

    public async Task<JobResponse?> GetJobByIdAsync(int id)
    {
        var job = await _jobRepository.GetByIdAsync(id);
        return job is null ? null : ToResponse(job);
    }

    public async Task<JobResponse?> CreateJobAsync(JobCreateRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title) ||
            string.IsNullOrWhiteSpace(request.CompanyName))
        {
            return null;
        }

        var job = new Job
        {
            Title = request.Title,
            CompanyName = request.CompanyName,
            Description = request.Description,
            Location = request.Location,
            Status = request.Status,
            Type = request.Type,
            RelatedUserId = request.RelatedUserId,
            CreatedAt = DateTime.UtcNow
        };

        await _jobRepository.AddAsync(job);
        await _jobRepository.SaveChangesAsync();

        var created = await _jobRepository.GetByIdAsync(job.Id);
        return created is null ? null : ToResponse(created);
    }

    public async Task<JobResponse?> UpdateJobAsync(int id, JobUpdateRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title) ||
            string.IsNullOrWhiteSpace(request.CompanyName))
        {
            return null;
        }

        var job = await _jobRepository.GetByIdForUpdateAsync(id);
        if (job is null) return null;

        job.Title = request.Title;
        job.CompanyName = request.CompanyName;
        job.Description = request.Description;
        job.Location = request.Location;
        job.Status = request.Status;
        job.Type = request.Type;
        job.RelatedUserId = request.RelatedUserId;

        await _jobRepository.SaveChangesAsync();

        var updated = await _jobRepository.GetByIdAsync(id);
        return updated is null ? null : ToResponse(updated);
    }

    public async Task<bool> DeleteJobAsync(int id)
    {
        var job = await _jobRepository.GetByIdForUpdateAsync(id);
        if (job is null) return false;

        _jobRepository.Delete(job);
        await _jobRepository.SaveChangesAsync();
        return true;
    }

    private static JobResponse ToResponse(Job job) => new(
        job.Id,
        job.Title,
        job.CompanyName,
        job.Description,
        job.Location,
        job.Status,
        job.Type,
        job.RelatedUserId,
        job.RelatedUser?.FullName ?? string.Empty,
        job.CreatedAt
    );
}
