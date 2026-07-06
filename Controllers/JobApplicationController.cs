using System.Security.Claims;
using JobTrackerApi.Data;
using JobTrackerApi.Dtos;
using JobTrackerApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobTrackerApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class JobApplicationsController : ControllerBase
{
    private readonly JobTrackerContext _context;

    private static readonly HashSet<string> AllowedStatuses = new(StringComparer.OrdinalIgnoreCase)
    {
        "Applied",
        "Interview",
        "Rejected",
        "Offer",
        "Accepted"
    };

    public JobApplicationsController(JobTrackerContext context)
    {
        _context = context;
    }

    private int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userIdClaim))
        {
            throw new UnauthorizedAccessException("User ID claim is missing.");
        }

        return int.Parse(userIdClaim);
    }

    private static bool IsValidStatus(string status)
    {
        return AllowedStatuses.Contains(status);
    }

    private static JobApplicationResponseDto ToResponseDto(JobApplication application)
    {
        return new JobApplicationResponseDto
        {
            Id = application.Id,
            Company = application.Company,
            Role = application.Role,
            Status = application.Status,
            AppliedDate = application.AppliedDate,
            Notes = application.Notes
        };
    }

    [AllowAnonymous]
    [HttpGet("statuses")]
    public ActionResult<List<string>> GetStatuses()
    {
        return Ok(AllowedStatuses.ToList());
    }

    [HttpGet]
    public async Task<ActionResult<List<JobApplicationResponseDto>>> GetAll()
    {
        var userId = GetCurrentUserId();

        var applications = await _context.JobApplications
            .Where(a => a.UserId == userId)
            .ToListAsync();

        var response = applications.Select(ToResponseDto).ToList();

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<JobApplicationResponseDto>> GetById(int id)
    {
        var userId = GetCurrentUserId();

        var application = await _context.JobApplications
            .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

        if (application == null)
        {
            return NotFound();
        }

        return Ok(ToResponseDto(application));
    }

    [HttpPost]
    public async Task<ActionResult<JobApplicationResponseDto>> Create(CreateJobApplicationDto newApplicationDto)
    {
        var userId = GetCurrentUserId();

        if (!IsValidStatus(newApplicationDto.Status))
        {
            return BadRequest(new
            {
                message = "Invalid status. Valid statuses are: Applied, Interview, Rejected, Offer, Accepted."
            });
        }

        var application = new JobApplication
        {
            Company = newApplicationDto.Company,
            Role = newApplicationDto.Role,
            Status = newApplicationDto.Status,
            AppliedDate = newApplicationDto.AppliedDate,
            Notes = newApplicationDto.Notes,
            UserId = userId
        };

        _context.JobApplications.Add(application);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = application.Id },
            ToResponseDto(application)
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateJobApplicationDto updatedApplicationDto)
    {
        var userId = GetCurrentUserId();

        if (!IsValidStatus(updatedApplicationDto.Status))
        {
            return BadRequest(new
            {
                message = "Invalid status. Valid statuses are: Applied, Interview, Rejected, Offer, Accepted."
            });
        }

        var application = await _context.JobApplications
            .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

        if (application == null)
        {
            return NotFound();
        }

        application.Company = updatedApplicationDto.Company;
        application.Role = updatedApplicationDto.Role;
        application.Status = updatedApplicationDto.Status;
        application.AppliedDate = updatedApplicationDto.AppliedDate;
        application.Notes = updatedApplicationDto.Notes;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetCurrentUserId();

        var application = await _context.JobApplications
            .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

        if (application == null)
        {
            return NotFound();
        }

        _context.JobApplications.Remove(application);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("status/{status}")]
    public async Task<ActionResult<List<JobApplicationResponseDto>>> GetByStatus(string status)
    {
        var userId = GetCurrentUserId();

        var applications = await _context.JobApplications
            .Where(a => a.UserId == userId && a.Status.ToLower() == status.ToLower())
            .ToListAsync();

        var response = applications.Select(ToResponseDto).ToList();

        return Ok(response);
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<JobApplicationResponseDto>>> Search(
        [FromQuery] string? company,
        [FromQuery] string? status)
    {
        var userId = GetCurrentUserId();

        var query = _context.JobApplications
            .Where(a => a.UserId == userId)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(company))
        {
            query = query.Where(a => a.Company.ToLower().Contains(company.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(a => a.Status.ToLower() == status.ToLower());
        }

        var applications = await query.ToListAsync();

        var response = applications.Select(ToResponseDto).ToList();

        return Ok(response);
    }
}