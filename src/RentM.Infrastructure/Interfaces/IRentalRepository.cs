using RentM.Domain.Models;

namespace RentM.Infrastructure.Interfaces
{
    public interface IRentalRepository
    {
        Task AddAsync(Rental rental);
        Task<Rental> GetByIdAsync(Guid id);
        Task UpdateAsync(Rental rental);
    }
}

