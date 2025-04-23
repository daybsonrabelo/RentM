using Microsoft.EntityFrameworkCore;
using RentM.Domain.Models;

namespace RentM.Infrastructure.Data
{
    public class RentMDbContext : DbContext
    {
        public RentMDbContext(DbContextOptions<RentMDbContext> options) : base(options) { }

        public DbSet<Rental> Rentals { get; set; }
        public DbSet<DeliveryPerson> DeliveryPersons { get; set; }
        public DbSet<Motorcycle> Motorcycles { get; set; }
        public DbSet<MotorcycleEvent> MotorcycleEvents { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}
