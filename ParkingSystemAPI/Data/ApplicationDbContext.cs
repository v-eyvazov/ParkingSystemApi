using Microsoft.EntityFrameworkCore;
using ParkingSystemAPI.Models;

namespace ParkingSystemAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


        public DbSet<ParkingLot> ParkingLots { get; set; }
        public DbSet<Activity> Activities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Activity>()
                .Property(p => p.TicketNumber)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<Activity>()
                .Property(d => d.CreatedTime)
                .HasDefaultValueSql("LOCALTIMESTAMP");
        }
    }
}
