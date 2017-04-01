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
    public class HeroesRepository
    {
        private readonly HeroesDbContext _context;

        public HeroesRepository(HeroesDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Hero>> GetAll()
        {
            return await _context.Heroes.ToListAsync();
        }

        public async Task<Hero> GetById(int id)
        {
            return await _context.Heroes.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Hero> Create(Hero model)
        {
            _context.Add(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<Hero> Update(Hero model)
        {
            _context.Update(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task Delete(int id)
        {
            var model = await GetById(id);
            _context.Remove(model);
            await _context.SaveChangesAsync();
        }
    }
}
