using Microsoft.EntityFrameworkCore;

using SWP391.Project.Entities;

namespace SWP391.Project.DbContexts
{
    public class ProjectDbContext : DbContext
    {
        public ProjectDbContext(DbContextOptions<ProjectDbContext> opts, IConfiguration configuration) : base(options: opts)
        {
        }

        public DbSet<AccountEntity> Accounts { get; set; } = null!;
    }
}