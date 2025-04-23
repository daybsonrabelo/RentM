using Microsoft.EntityFrameworkCore;
using RentM.Domain.Models;
using RentM.Infrastructure.Data;
using RentM.Infrastructure.Interfaces;

namespace RentM.Infrastructure.Repositories
{
    public class DeliveryPersonRepository : IDeliveryPersonRepository
    {
        private readonly RentMDbContext _context;

        public DeliveryPersonRepository(RentMDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(DeliveryPerson deliveryPerson)
        {
            await _context.DeliveryPersons.AddAsync(deliveryPerson);
            await _context.SaveChangesAsync();
        }

        public async Task<DeliveryPerson> GetByIdAsync(Guid id)
        {
            return await _context.DeliveryPersons.FirstOrDefaultAsync(dp => dp.Id == id);
        }

        public async Task<DeliveryPerson> GetByCnpjAsync(string cnpj)
        {
            return await _context.DeliveryPersons.FirstOrDefaultAsync(dp => dp.Cnpj == cnpj);
        }

        public async Task<DeliveryPerson> GetByDriverLicenseNumberAsync(string driverLicenseNumber)
        {
            return await _context.DeliveryPersons.FirstOrDefaultAsync(dp => dp.DriverLicenseNumber == driverLicenseNumber);
        }

        public async Task UpdateAsync(DeliveryPerson deliveryPerson)
        {
            _context.DeliveryPersons.Update(deliveryPerson);
            await _context.SaveChangesAsync();
        }
    }
}

