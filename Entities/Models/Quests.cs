using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Entities.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class Quest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Completed { get; set; }

        public int HeroId { get; set; }
        public virtual Hero Hero { get; set; }
    }
}