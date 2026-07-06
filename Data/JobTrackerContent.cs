using Microsoft.EntityFrameworkCore;
using JobTrackerApi.Models;

namespace JobTrackerApi.Data;

public class JobTrackerContext : DbContext
{
    public JobTrackerContext(DbContextOptions<JobTrackerContext> options)
        : base(options)
    {
    }

    public DbSet<JobApplication> JobApplications => Set<JobApplication>();
    public DbSet<User> Users => Set<User>();
}