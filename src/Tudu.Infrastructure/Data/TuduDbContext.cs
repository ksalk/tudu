using Microsoft.EntityFrameworkCore;
using Tudu.Domain.Entities;

namespace Tudu.Infrastructure.Data
{
    public class TuduDbContext : DbContext
    {
        public DbSet<Todo> Todos { get; private set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseCosmos(
                "https://localhost:8081",
                "secret",
                "tudu-dev");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TuduDbContext).Assembly);
        }
    }
}
