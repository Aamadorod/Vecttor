using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using Vecttor02.Models;

namespace Vecttor02.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<TopAsteroidData> TopAsteroids { get; set; }
        public DbSet<AsteroidData> Asteroids { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Relación entre TopAsteroid y Asteroid
            modelBuilder.Entity<AsteroidData>()
                .HasOne(a => a.TopAsteroid)
                .WithMany(t => t.Asteroids)
                .HasForeignKey(a => a.TopAsteroidId);
        }
    }
}
