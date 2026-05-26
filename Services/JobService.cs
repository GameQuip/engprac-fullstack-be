using Backend.DTOs.Job;
using Backend.Repositories;

namespace Backend.Services;

public class JobService
{
    private readonly JobRepository _jobRepository;

    public JobService(JobRepository jobRepository)
    {
        _jobRepository = jobRepository;
    }

    public async Task<JobResponse?> UpdateJobAsync(int id, JobUpdateRequest request)
    {
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
}
