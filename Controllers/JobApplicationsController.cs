using Backend.Data;
using Backend.DTOs.JobApplication;
using Backend.Infrastructure;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[ApiController]
[Route("api/v1/applications")]
public class JobApplicationsController : ControllerBase
{
    private const string ApplicationStatusApplied = "Applied";
    private const string ApplicationStatusPassed = "Passed";
    private const string ApplicationStatusFailed = "Failed";

    private readonly AppDbContext _context;

    public JobApplicationsController(AppDbContext context)
    {
        _context = context;
    }

    private bool TryResolveCurrentUserId(out int userId, out IActionResult? errorResult)
    {
        userId = 0;
        errorResult = null;

        var userIdValue = Request.Headers["X-User-Id"].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(userIdValue))
        {
            errorResult = Unauthorized("Missing X-User-Id header");
            return false;
        }

        if (!int.TryParse(userIdValue, out userId))
        {
            errorResult = BadRequest("Invalid X-User-Id header");
            return false;
        }

        return true;
    }

    private static bool IsFinalStatus(string? status)
    {
        return string.Equals(status, ApplicationStatusPassed, StringComparison.OrdinalIgnoreCase)
            || string.Equals(status, ApplicationStatusFailed, StringComparison.OrdinalIgnoreCase);
    }

    [HttpPost]
    public async Task<IActionResult> ApplyJob([FromBody] ApplyJobRequest request)
    {
        if (!TryResolveCurrentUserId(out var userId, out var errorResult))
        {
            return errorResult!;
        }

        var jobExists = await _context.Jobs.AnyAsync(j => j.Id == request.JobId);
        var userExists = await _context.Users.AnyAsync(u => u.Id == userId);

        if (!jobExists)
        {
            return BadRequest("Job does not exist");
        }

        if (!userExists)
        {
            return BadRequest("User does not exist");
        }

        var alreadyApplied = await _context.JobApplications
            .AnyAsync(a => a.JobId == request.JobId && a.UserId == userId);

        if (alreadyApplied)
        {
            return BadRequest("User already applied to this job");
        }

        var application = new JobApplication
        {
            JobId = request.JobId,
            UserId = userId,
            Status = ApplicationStatusApplied,
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
    [RoleAuthorize("Admin")]
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

    [HttpGet("my")]
    public async Task<IActionResult> GetMyApplications()
    {
        if (!TryResolveCurrentUserId(out var userId, out var errorResult))
        {
            return errorResult!;
        }

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

    [HttpDelete("my/{id:int}")]
    public async Task<IActionResult> DeleteMyApplication(int id)
    {
        if (!TryResolveCurrentUserId(out var userId, out var errorResult))
        {
            return errorResult!;
        }

        var application = await _context.JobApplications
            .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

        if (application is null)
        {
            return NotFound();
        }

        if (!string.Equals(application.Status, ApplicationStatusApplied, StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest("Only applications with Applied status can be deleted");
        }

        _context.JobApplications.Remove(application);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPatch("{id:int}/status")]
    [RoleAuthorize("Admin")]
    public async Task<IActionResult> UpdateApplicationStatus(int id, [FromBody] UpdateJobApplicationStatusRequest request)
    {
        var application = await _context.JobApplications
            .FirstOrDefaultAsync(a => a.Id == id);

        if (application is null)
        {
            return NotFound();
        }

        if (!IsFinalStatus(request.Status))
        {
            return BadRequest("Status must be Passed or Failed");
        }

        application.Status = request.Status.Trim();

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
}