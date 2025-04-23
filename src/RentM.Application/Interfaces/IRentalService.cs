using RentM.Domain.ValueObjects;

namespace RentM.Application.Interfaces
{
    public interface IRentalService
    {
        Task RentMotorcycleAsync(Guid deliveryPersonId, Guid motorcycleId, RentalPlan plan, DateTime startDate);
        Task<decimal> CalculateRentalCostAsync(Guid rentalId, DateTime returnDate);
    }
}

