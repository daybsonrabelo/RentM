using System;
using System.Threading.Tasks;
using Moq;
using RentM.Application.Services;
using RentM.Domain.Models;
using RentM.Domain.ValueObjects;
using RentM.Infrastructure.Interfaces;
using Xunit;

namespace RentM.Tests
{
    public class RentalServiceTests
    {
        private readonly Mock<IRentalRepository> _rentalRepositoryMock;
        private readonly Mock<IDeliveryPersonRepository> _deliveryPersonRepositoryMock;
        private readonly Mock<IMotorcycleRepository> _motorcycleRepositoryMock;
        private readonly RentalService _rentalService;

        public RentalServiceTests()
        {
            _rentalRepositoryMock = new Mock<IRentalRepository>();
            _deliveryPersonRepositoryMock = new Mock<IDeliveryPersonRepository>();
            _motorcycleRepositoryMock = new Mock<IMotorcycleRepository>();
            _rentalService = new RentalService(
                _rentalRepositoryMock.Object,
                _deliveryPersonRepositoryMock.Object,
                _motorcycleRepositoryMock.Object
            );
        }

        [Fact]
        public async Task RentMotorcycleAsync_ShouldThrowException_WhenDeliveryPersonNotFound()
        {
            // Arrange
            var deliveryPersonId = Guid.NewGuid();
            var motorcycleId = Guid.NewGuid();
            var plan = new RentalPlan { Days = 7, DailyRate = 30 };
            var startDate = DateTime.UtcNow.Date.AddDays(1);

            _deliveryPersonRepositoryMock
                .Setup(repo => repo.GetByIdAsync(deliveryPersonId))
                .ReturnsAsync((DeliveryPerson)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                _rentalService.RentMotorcycleAsync(deliveryPersonId, motorcycleId, plan, startDate));
            Assert.Equal("Delivery person not found", exception.Message);
        }

        [Fact]
        public async Task RentMotorcycleAsync_ShouldThrowException_WhenMotorcycleNotFound()
        {
            // Arrange
            var deliveryPersonId = Guid.NewGuid();
            var motorcycleId = Guid.NewGuid();
            var plan = new RentalPlan { Days = 7, DailyRate = 30 };
            var startDate = DateTime.UtcNow.Date.AddDays(1);

            _deliveryPersonRepositoryMock
                .Setup(repo => repo.GetByIdAsync(deliveryPersonId))
                .ReturnsAsync(new DeliveryPerson { DriverLicenseType = "A" });

            _motorcycleRepositoryMock
                .Setup(repo => repo.GetByIdAsync(motorcycleId))
                .ReturnsAsync((Motorcycle)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                _rentalService.RentMotorcycleAsync(deliveryPersonId, motorcycleId, plan, startDate));
            Assert.Equal("Motorcycle not found", exception.Message);
        }

        [Fact]
        public async Task RentMotorcycleAsync_ShouldThrowException_WhenStartDateIsInvalid()
        {
            // Arrange
            var deliveryPersonId = Guid.NewGuid();
            var motorcycleId = Guid.NewGuid();
            var plan = new RentalPlan { Days = 7, DailyRate = 30 };
            var startDate = DateTime.UtcNow.Date; // Invalid start date (not one day after today)

            _deliveryPersonRepositoryMock
                .Setup(repo => repo.GetByIdAsync(deliveryPersonId))
                .ReturnsAsync(new DeliveryPerson { DriverLicenseType = "A" });

            _motorcycleRepositoryMock
                .Setup(repo => repo.GetByIdAsync(motorcycleId))
                .ReturnsAsync(new Motorcycle());

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                _rentalService.RentMotorcycleAsync(deliveryPersonId, motorcycleId, plan, startDate));
            Assert.Contains("The start date must be exactly one day after the current date", exception.Message);
        }

        [Fact]
        public async Task CalculateRentalCostAsync_ShouldReturnTotalCost_WhenReturnedOnTime()
        {
            // Arrange
            var rentalId = Guid.NewGuid();
            var returnDate = DateTime.UtcNow.Date.AddDays(7);

            var rental = new Rental
            {
                Id = rentalId,
                StartDate = DateTime.UtcNow.Date,
                ExpectedEndDate = DateTime.UtcNow.Date.AddDays(7),
                TotalCost = 210 // 7 days * 30 per day
            };

            _rentalRepositoryMock
                .Setup(repo => repo.GetByIdAsync(rentalId))
                .ReturnsAsync(rental);

            // Act
            var totalCost = await _rentalService.CalculateRentalCostAsync(rentalId, returnDate);

            // Assert
            Assert.Equal(210, totalCost);
        }

        [Fact]
        public async Task CalculateRentalCostAsync_ShouldApplyPenalty_WhenReturnedEarly()
        {
            // Arrange
            var rentalId = Guid.NewGuid();
            var returnDate = DateTime.UtcNow.Date.AddDays(5); // Returned 2 days early

            var rental = new Rental
            {
                Id = rentalId,
                StartDate = DateTime.UtcNow.Date,
                ExpectedEndDate = DateTime.UtcNow.Date.AddDays(7),
                TotalCost = 210 // 7 days * 30 per day
            };

            _rentalRepositoryMock
                .Setup(repo => repo.GetByIdAsync(rentalId))
                .ReturnsAsync(rental);

            // Act
            var totalCost = await _rentalService.CalculateRentalCostAsync(rentalId, returnDate);

            // Assert
            var expectedPenalty = 2 * (30 * 0.20m); // 2 days * 20% penalty
            var expectedTotal = 210 - (2 * 30) + expectedPenalty;
            Assert.Equal(expectedTotal, totalCost);
        }

        [Fact]
        public async Task CalculateRentalCostAsync_ShouldApplyExtraCost_WhenReturnedLate()
        {
            // Arrange
            var rentalId = Guid.NewGuid();
            var returnDate = DateTime.UtcNow.Date.AddDays(9); // Returned 2 days late

            var rental = new Rental
            {
                Id = rentalId,
                StartDate = DateTime.UtcNow.Date,
                ExpectedEndDate = DateTime.UtcNow.Date.AddDays(7),
                TotalCost = 210 // 7 days * 30 per day
            };

            _rentalRepositoryMock
                .Setup(repo => repo.GetByIdAsync(rentalId))
                .ReturnsAsync(rental);

            // Act
            var totalCost = await _rentalService.CalculateRentalCostAsync(rentalId, returnDate);

            // Assert
            var expectedExtraCost = 2 * 50; // 2 extra days * 50 per day
            var expectedTotal = 210 + expectedExtraCost;
            Assert.Equal(expectedTotal, totalCost);
        }
    }
}
