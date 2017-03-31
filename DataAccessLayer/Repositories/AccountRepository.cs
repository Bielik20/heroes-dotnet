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

namespace DataAccessLayer.Repositories
{
    public class AccountRepository
    {
        private readonly HeroesDbContext _context;

        public AccountRepository(HeroesDbContext context)
        {
            _context = context;
        }

        public async Task<ApplicationUser> VerifyUser(UserVM model)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.UserName == model.UserName && x.PasswordHash == GetHash(model.Password));
        }

        public async Task<ApplicationUser> CreateUser(UserVM model)
        {
            if(await _context.Users.AnyAsync(x => x.UserName == model.UserName))
            {
                throw new Exception("User with that name already exists");
            }
            
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                PasswordHash = GetHash(model.Password) 
            };
            _context.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        private string GetHash(string password)
        {
            using(var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashBytes);
            }
        }
    }
}
