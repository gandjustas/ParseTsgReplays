using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParseTsgReplays
{
    public class ReplaysContext:DbContext
    {
        public DbSet<Replay> Replays { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<EnterExitEvent> EnterExitEvents { get; set; }
        public DbSet<Kill> Kills { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=replays.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Unit>()
                .HasKey("ReplayId", "Id");

            base.OnModelCreating(modelBuilder);
        }
    }
}
