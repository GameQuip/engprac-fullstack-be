using Backend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[ApiController]
[Route("api/v1/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly AppDbContext _context;

    public DashboardController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        var totalJobs = await _context.Jobs.CountAsync();
        var totalUsers = await _context.Users.CountAsync();
        var totalApplications = await _context.JobApplications.CountAsync();

        var appliedCount = await _context.JobApplications
            .CountAsync(a => a.Status == "Applied");

        return Ok(new
        {
            totalJobs,
            totalUsers,
            totalApplications,
            appliedCount
        });
    }
}