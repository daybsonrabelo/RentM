using RentM.Application.Interfaces;
using RentM.Domain.Models;
using RentM.Domain.ValueObjects;
using RentM.Infrastructure.Interfaces;
using RentM.Infrastructure.Repositories;

namespace RentM.Application.Services
{
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IDeliveryPersonRepository _deliveryPersonRepository;
        private readonly IMotorcycleRepository _motorcycleRepository;

        public RentalService(
            IRentalRepository rentalRepository,
            IDeliveryPersonRepository deliveryPersonRepository,
            IMotorcycleRepository motorcycleRepository)
        {
            _rentalRepository = rentalRepository;
            _deliveryPersonRepository = deliveryPersonRepository;
            _motorcycleRepository = motorcycleRepository;
        }

        public async Task RentMotorcycleAsync(Guid deliveryPersonId, Guid motorcycleId, RentalPlan plan, DateTime startDate)
        {
            // 1. Validate if the delivery person exists and is eligible
            var deliveryPerson = await _deliveryPersonRepository.GetByIdAsync(deliveryPersonId);
            if (deliveryPerson == null)
                throw new Exception("Delivery person not found");

            if (deliveryPerson.DriverLicenseType != "A" && deliveryPerson.DriverLicenseType != "A+B")
                throw new Exception("Delivery person is not eligible to rent a motorcycle (requires category A license)");

            // 2. Validate if the motorcycle exists and is available
            var motorcycle = await _motorcycleRepository.GetByIdAsync(motorcycleId);
            if (motorcycle == null)
                throw new Exception("Motorcycle not found");

            var existingRental = await _rentalRepository.GetByIdAsync(motorcycleId);
            if (existingRental != null && existingRental.ActualEndDate == null)
                throw new Exception("Motorcycle is currently rented and not available");

            // 3. Validate the rental plan
            if (plan.Days <= 0 || plan.DailyRate <= 0)
                throw new Exception("Invalid rental plan");

            // 4. Validate the start date
            var expectedStartDate = DateTime.UtcNow.Date.AddDays(1); // One day after the current date
            if (startDate.Date != expectedStartDate)
                throw new Exception($"The start date must be exactly one day after the current date. Expected: {expectedStartDate:yyyy-MM-dd}");

            // 5. Calculate the expected end date
            var expectedEndDate = startDate.AddDays(plan.Days);

            var rental = new Rental
            {
                Id = Guid.NewGuid(),
                DeliveryPersonId = deliveryPersonId,
                MotorcycleId = motorcycleId,
                StartDate = startDate,
                ExpectedEndDate = expectedEndDate,
                TotalCost = plan.Days * plan.DailyRate
            };

            await _rentalRepository.AddAsync(rental);
        }

        public async Task<decimal> CalculateRentalCostAsync(Guid rentalId, DateTime returnDate)
        {
            var rental = await _rentalRepository.GetByIdAsync(rentalId);
            if (rental == null) throw new Exception("Rental not found");

            // If the return date is before or on the expected end date
            if (returnDate <= rental.ExpectedEndDate)
            {
                var daysNotUsed = (rental.ExpectedEndDate - returnDate).Days;

                if (daysNotUsed > 0)
                {
                    // Determine the penalty percentage based on the rental plan
                    decimal penaltyPercentage = 0;
                    var rentalDays = (rental.ExpectedEndDate - rental.StartDate).Days;

                    if (rentalDays == 7)
                        penaltyPercentage = 0.20m; // 20% for 7-day plan
                    else if (rentalDays == 15)
                        penaltyPercentage = 0.40m; // 40% for 15-day plan

                    // Calculate the penalty for unused days
                    var penalty = daysNotUsed * (rental.TotalCost / rentalDays) * penaltyPercentage;
                    return rental.TotalCost - (daysNotUsed * (rental.TotalCost / rentalDays)) + penalty;
                }

                return rental.TotalCost; // No penalty if all days are used
            }

            // If the return date is after the expected end date
            var extraDays = (returnDate - rental.ExpectedEndDate).Days;
            var extraCost = extraDays * 50; // R$50,00 per extra day
            return rental.TotalCost + extraCost;
        }

    }
}

