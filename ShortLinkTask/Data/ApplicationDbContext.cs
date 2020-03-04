using Microsoft.EntityFrameworkCore;
using ShortLinkTask.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShortLinkTask.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<Tracks> Tracks { get; set; }
        public DbSet<ShortUrls> ShortUrls { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

    }
}
