using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Loan> Loans => Set<Loan>();

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public void BeginTransaction()
        {
            Database.BeginTransaction();
        }
        public void CommitTransaction()
        {
            if (Database.CurrentTransaction != null)
            {
                Database.CommitTransaction();
            }
        }
        public void RollbackTransaction()
        {
            if (Database.CurrentTransaction != null)
            {
                Database.RollbackTransaction();
            }
        }
    }

}