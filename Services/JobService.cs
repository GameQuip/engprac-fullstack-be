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

        return jobs.Select(job => new JobResponse(
            job.Id,
            job.Title,
            job.CompanyName,
            job.Description,
            job.Location,
            job.CreatedAt
        )).ToList();
    }

    public async Task<JobResponse?> GetJobByIdAsync(int id)
    {
        var job = await _jobRepository.GetByIdAsync(id);

        if (job is null)
        {
            return null;
        }

        return new JobResponse(
            job.Id,
            job.Title,
            job.CompanyName,
            job.Description,
            job.Location,
            job.CreatedAt
        );
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
            CreatedAt = DateTime.UtcNow
        };

        await _jobRepository.AddAsync(job);
        await _jobRepository.SaveChangesAsync();

        return new JobResponse(
            job.Id,
            job.Title,
            job.CompanyName,
            job.Description,
            job.Location,
            job.CreatedAt
        );
    }

    public async Task<JobResponse?> UpdateJobAsync(int id, JobUpdateRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title) ||
            string.IsNullOrWhiteSpace(request.CompanyName))
        {
            return null;
        }

        var job = await _jobRepository.GetByIdForUpdateAsync(id);

        if (job is null)
        {
            return null;
        }

        job.Title = request.Title;
        job.CompanyName = request.CompanyName;
        job.Description = request.Description;
        job.Location = request.Location;

        await _jobRepository.SaveChangesAsync();

        return new JobResponse(
            job.Id,
            job.Title,
            job.CompanyName,
            job.Description,
            job.Location,
            job.CreatedAt
        );
    }

    public async Task<bool> DeleteJobAsync(int id)
    {
        var job = await _jobRepository.GetByIdForUpdateAsync(id);

        if (job is null)
        {
            return false;
        }

        _jobRepository.Delete(job);
        await _jobRepository.SaveChangesAsync();

        return true;
    }
}