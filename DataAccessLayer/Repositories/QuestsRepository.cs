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
    public class QuestsRepository
    {
        private readonly HeroesDbContext _context;

        public QuestsRepository(HeroesDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Quest>> GetAll()
        {
            return await _context.Quests.ToListAsync();
        }

        public async Task<IEnumerable<Quest>> GetOwnedByHero(int heroId)
        {
            return await _context.Quests.Where(x => x.HeroId == heroId).ToListAsync();
        }

        public async Task<Quest> GetById(int id)
        {
            return await _context.Quests.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Quest> Create(Quest model)
        {
            _context.Add(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<Quest> Update(Quest model)
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
