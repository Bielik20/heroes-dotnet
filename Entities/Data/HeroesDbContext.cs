using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Entities.Data
{
    public class HeroesDbContext : DbContext
    {
        // ### From this project
        // dotnet ef --startup-project ../Server
        // dotnet ef --startup-project ../Server migrations add InitialCreate
        // dotnet ef --startup-project ../Server database update

        public HeroesDbContext(DbContextOptions<HeroesDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<Quest>()
                .HasOne(q => q.Hero)
                .WithMany(h => h.Quests)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Hero> Heroes { get; set; }
        public DbSet<Quest> Quests { get; set; }
    }
}
