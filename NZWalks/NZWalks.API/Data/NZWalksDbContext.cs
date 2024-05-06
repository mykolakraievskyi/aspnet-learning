using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Data
{
    public class NZWalksDbContext : DbContext
    {
        public NZWalksDbContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions) // needed to get a connection string from appsettings
        {
            
        }

        public DbSet<Difficulty> Difficulties { get; set; } // represent collection inside a database
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }
    }
}
