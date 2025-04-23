using Microsoft.EntityFrameworkCore;
using RentM.Domain.Models;
using RentM.Infrastructure.Data;
using RentM.Infrastructure.Interfaces;

namespace RentM.Infrastructure.Repositories
{
    public class MotorcycleRepository : IMotorcycleRepository
    {
        private readonly RentMDbContext _context;

        public MotorcycleRepository(RentMDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Motorcycle motorcycle)
        {
            await _context.Motorcycles.AddAsync(motorcycle);
            await _context.SaveChangesAsync();
        }

        public async Task<Motorcycle> GetByIdAsync(Guid id)
        {
            return await _context.Motorcycles.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Motorcycle>> GetByLicensePlateAsync(string licensePlate)
        {
            return await _context.Motorcycles
                .Where(m => m.LicensePlate.Contains(licensePlate))
                .ToListAsync();
        }

        public async Task UpdateAsync(Motorcycle motorcycle)
        {
            _context.Motorcycles.Update(motorcycle);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var motorcycle = await _context.Motorcycles.FirstOrDefaultAsync(m => m.Id == id);
            if (motorcycle != null)
            {
                _context.Motorcycles.Remove(motorcycle);
                await _context.SaveChangesAsync();
            }
        }
    }
}
