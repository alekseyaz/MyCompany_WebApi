using Microsoft.EntityFrameworkCore;
using MyCompany.NameProject.Infrastructure.Persistence.Configurations;
using MyCompany.NameProject.Infrastructure.Persistence.Models;

namespace MyCompany.NameProject.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<WeatherHistoryEntity> WeatherHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new WeatherHistoryEntityConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
