using Backend.DTOs.Job;
using Backend.Infrastructure;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/v1/jobs")]
public class JobsController : ControllerBase
{
    private readonly JobService _jobService;

    public JobsController(JobService jobService)
    {
        _jobService = jobService;
    }

    [HttpGet]
    public async Task<ActionResult<List<JobResponse>>> GetAllJobs()
    {
        var jobs = await _jobService.GetAllJobsAsync();
        return Ok(jobs);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<JobResponse>> GetJobById(int id)
    {
        var job = await _jobService.GetJobByIdAsync(id);

        if (job is null)
        {
            return NotFound();
        }

        return Ok(job);
    }

    [HttpPost]
    [RoleAuthorize("Admin")]
    public async Task<ActionResult<JobResponse>> CreateJob([FromBody] JobCreateRequest request)
    {
        var createdJob = await _jobService.CreateJobAsync(request);

        if (createdJob is null)
        {
            return BadRequest("Title and CompanyName are required");
        }

        return CreatedAtAction(nameof(GetJobById), new { id = createdJob.Id }, createdJob);
    }

    [HttpPut("{id:int}")]
    [RoleAuthorize("Admin")]
    public async Task<ActionResult<JobResponse>> UpdateJob(int id, [FromBody] JobUpdateRequest request)
    {
        var updatedJob = await _jobService.UpdateJobAsync(id, request);

        if (updatedJob is null)
        {
            return NotFound("Job not found or invalid data");
        }

        return Ok(updatedJob);
    }

    [HttpDelete("{id:int}")]
    [RoleAuthorize("Admin")]
    public async Task<IActionResult> DeleteJob(int id)
    {
        var deleted = await _jobService.DeleteJobAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}