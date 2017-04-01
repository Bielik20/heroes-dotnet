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
    }
}
