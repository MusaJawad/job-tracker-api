using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobTrackerApi.Data;
using JobTrackerApi.Models;

namespace JobTrackerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobApplicationsController : ControllerBase
{
    private readonly JobTrackerContext _context;

    public JobApplicationsController(JobTrackerContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<JobApplication>>> GetAll()
    {
        return await _context.JobApplications.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<JobApplication>> GetById(int id)
    {
        var application = await _context.JobApplications.FindAsync(id);

        if (application == null)
        {
            return NotFound();
        }

        return application;
    }

    [HttpPost]
    public async Task<ActionResult<JobApplication>> Create(JobApplication newApplication)
    {
        _context.JobApplications.Add(newApplication);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = newApplication.Id }, newApplication);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, JobApplication updatedApplication)
    {
        var application = await _context.JobApplications.FindAsync(id);

        if (application == null)
        {
            return NotFound();
        }

        application.Company = updatedApplication.Company;
        application.Role = updatedApplication.Role;
        application.Status = updatedApplication.Status;
        application.AppliedDate = updatedApplication.AppliedDate;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var application = await _context.JobApplications.FindAsync(id);

        if (application == null)
        {
            return NotFound();
        }

        _context.JobApplications.Remove(application);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}