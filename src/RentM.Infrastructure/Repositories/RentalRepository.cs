using Microsoft.EntityFrameworkCore;
using RentM.Domain.Models;
using RentM.Infrastructure.Data;
using RentM.Infrastructure.Interfaces;

namespace RentM.Infrastructure.Repositories
{
    public class RentalRepository : IRentalRepository
    {
        private readonly RentMDbContext _context;

        public RentalRepository(RentMDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Rental rental)
        {
            await _context.Rentals.AddAsync(rental);
            await _context.SaveChangesAsync();
        }

        public async Task<Rental> GetByIdAsync(Guid id)
        {
            return await _context.Rentals.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task UpdateAsync(Rental rental)
        {
            _context.Rentals.Update(rental);
            await _context.SaveChangesAsync();
        }
    }
}
