using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Entities.Data;
using Entities.Models;
using Entities.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Entities.Data
{
    public class SeedData
    {
        public static void Initialize(HeroesDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Users.Any())
            {
                return;
            }

            var user = new ApplicationUser
            {
                UserName = "Sample User",
                PasswordHash = GetHash("sample123")
            };
            context.Add(user);
            context.SaveChanges();

            var heroes = new Hero[]
            {
                new Hero { Name = "First Hero" },
                new Hero { Name = "Magneta" },
                new Hero { Name = "Spiderman" }
            };
            context.AddRange(heroes);
            context.SaveChanges();

            var quests = new Quest[]
            {
                new Quest { Title = "Very first quest", Completed = true, HeroId = heroes[0].Id },
                new Quest { Title = "Sky fall in Skyrim", Completed = true, HeroId = heroes[0].Id },
                new Quest { Title = "With friends like this", Completed = true, HeroId = heroes[0].Id },
                new Quest { Title = "The last resort", Completed = true, HeroId = heroes[2].Id },
                new Quest { Title = "Collect some garbage", Completed = true, HeroId = heroes[2].Id },
                new Quest { Title = "Talk with Diego", Completed = true, HeroId = heroes[2].Id },
                new Quest { Title = "Solidate our relations with Papal State", Completed = true, HeroId = heroes[2].Id },
                new Quest { Title = "Enemy of enemy", Completed = true },
                new Quest { Title = "Attack HRE", Completed = true }
            };
            context.AddRange(quests);
            context.SaveChanges();
        }

        private static string GetHash(string password)
        {
            using(var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashBytes);
            }
        }
    }
}
