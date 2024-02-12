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
                "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
                "tudu-dev");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TuduDbContext).Assembly);
        }
    }
}
