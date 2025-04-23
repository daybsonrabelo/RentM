using RentM.Domain.Models;

namespace RentM.Infrastructure.Interfaces
{
    public interface IMotorcycleRepository
    {
        Task AddAsync(Motorcycle motorcycle);
        Task<Motorcycle> GetByIdAsync(Guid id);
        Task<IEnumerable<Motorcycle>> GetByLicensePlateAsync(string licensePlate);
        Task UpdateAsync(Motorcycle motorcycle);
        Task DeleteAsync(Guid id);
    }
}

