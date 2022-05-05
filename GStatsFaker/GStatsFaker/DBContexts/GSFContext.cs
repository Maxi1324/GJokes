using Bogus;
using GStatsFaker.Model;
using Microsoft.EntityFrameworkCore;

namespace GStatsFaker.DBContexts
{
    public class GSFContext:DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<EmalVerifikation> EmalVerifikations => Set<EmalVerifikation>();
        public DbSet<Blocked> BlockList => Set<Blocked>();
        public GSFContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
           Seed();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }


        public void Seed(int n = 100)
        {
            var EmailVerifications = new Faker<EmalVerifikation>()
                .RuleFor(e => e.Erstellt, (f, u) => u.Erstellt = f.Date.Between(DateTime.Now, DateTime.Now.AddMonths(-100)))
                .RuleFor(e => e.Code, (f, u) => u.Code = f.Random.String(10))
                .RuleFor(e => e.IsVerifiziert, (f, e) => e.IsVerifiziert = f.Random.Bool(.7f));

            var Users1 = new Faker<User>()
                .RuleFor(u => u.Email, (f, u) => u.Email = f.Internet.Email())
                .RuleFor(u => u.Created, (f, u) => u.Created = f.Date.Between(DateTime.Now, DateTime.Now.AddMonths(-100)))
                .RuleFor(u => u.Password, (f, u) => u.Password = f.Internet.Password(100))
                .RuleFor(u => u.Id, (f, u) => u.Id = f.UniqueIndex)
                .RuleFor(u => u.ConSettings, (f, u) => new ConSettings()
                {
                    GithubEmail = f.Internet.Email(),
                    RepoName = f.Random.String(20),
                    GithubUsername = f.Name.FirstName(),
                    MaxCon = f.Random.Int(20, 30),
                    MinCon = f.Random.Int(0, 10)
                })
                .RuleFor(u => u.EmalVerifikations, (f, u) => EmailVerifications.GenerateBetween(0, 6).ToList());


            Random rand = new Random();
            List<User> us = Users1.Generate(n);

            List<Blocked> Blocklist1 = new List<Blocked>();
            for(int i = 0; i < n * 0.05f;i++)
            {
                User u = us[i];
                Blocklist1.Add(new Blocked (){BlockTime = DateTime.Now.AddDays(-rand.Next(300)),Email = u.Email});
            }

            Users.AddRange(us);
            BlockList.AddRange(Blocklist1);
            SaveChanges();
        }
    }
}
