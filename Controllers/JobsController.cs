using Backend.DTOs.Job;
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

    [HttpPut("{id:int}")]
    public async Task<ActionResult<JobResponse>> UpdateJob(int id, [FromBody] JobUpdateRequest request)
    {
        var updatedJob = await _jobService.UpdateJobAsync(id, request);
        if (updatedJob is null)
        {
            return NotFound();
        }

        return Ok(updatedJob);
    }
}
