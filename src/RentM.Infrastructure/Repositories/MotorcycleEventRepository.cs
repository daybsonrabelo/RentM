using Microsoft.EntityFrameworkCore;
using RentM.Infrastructure.Data;
using RentM.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentM.Infrastructure.Repositories
{
    public class MotorcycleEventRepository : IMotorcycleEventRepository
    {
        private readonly RentMDbContext _context;

        public MotorcycleEventRepository(RentMDbContext context)
        {
            _context = context;
        }

        public async Task SaveEventAsync(MotorcycleEvent motorcycleEvent)
        {
            await _context.MotorcycleEvents.AddAsync(motorcycleEvent);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<MotorcycleEvent>> GetAllEventsAsync()
        {
            return await _context.MotorcycleEvents.ToListAsync();
        }
    }
}