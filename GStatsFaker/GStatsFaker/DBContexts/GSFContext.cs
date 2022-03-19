using GStatsFaker.Model;
using Microsoft.EntityFrameworkCore;

namespace GStatsFaker.DBContexts
{
    public class GSFContext:DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<EmalVerifikation> EmalVerifikations => Set<EmalVerifikation>();
        public GSFContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
