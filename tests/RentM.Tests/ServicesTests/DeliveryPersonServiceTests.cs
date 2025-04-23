using System;
using System.Threading.Tasks;
using Moq;
using RentM.Application.DTOs;
using RentM.Application.Services;
using RentM.Domain.Models;
using RentM.Infrastructure.Interfaces;
using Xunit;

namespace RentM.Tests
{
    public class DeliveryPersonServiceTests
    {
        private readonly Mock<IDeliveryPersonRepository> _deliveryPersonRepositoryMock;
        private readonly DeliveryPersonService _deliveryPersonService;

        public DeliveryPersonServiceTests()
        {
            _deliveryPersonRepositoryMock = new Mock<IDeliveryPersonRepository>();
            _deliveryPersonService = new DeliveryPersonService(_deliveryPersonRepositoryMock.Object);
        }

        [Fact]
        public async Task RegisterDeliveryPersonAsync_ShouldThrowException_WhenCnhTypeIsInvalid()
        {
            // Arrange
            var deliveryPersonDto = new DeliveryPersonDto
            {
                DriverLicenseType = "C" // Invalid CNH type
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                _deliveryPersonService.RegisterDeliveryPersonAsync(deliveryPersonDto));
            Assert.Equal("Invalid CNH type. Valid types are: A, B, or A+B.", exception.Message);
        }

        [Fact]
        public async Task RegisterDeliveryPersonAsync_ShouldThrowException_WhenCnpjAlreadyExists()
        {
            // Arrange
            var deliveryPersonDto = new DeliveryPersonDto
            {
                Cnpj = "12345678901234",
                DriverLicenseType = "A"
            };

            _deliveryPersonRepositoryMock
                .Setup(repo => repo.GetByCnpjAsync(deliveryPersonDto.Cnpj))
                .ReturnsAsync(new DeliveryPerson());

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                _deliveryPersonService.RegisterDeliveryPersonAsync(deliveryPersonDto));
            Assert.Equal("A delivery person with this CNPJ already exists.", exception.Message);
        }

        [Fact]
        public async Task RegisterDeliveryPersonAsync_ShouldThrowException_WhenDriverLicenseNumberAlreadyExists()
        {
            // Arrange
            var deliveryPersonDto = new DeliveryPersonDto
            {
                DriverLicenseNumber = "123456789",
                DriverLicenseType = "A"
            };

            _deliveryPersonRepositoryMock
                .Setup(repo => repo.GetByDriverLicenseNumberAsync(deliveryPersonDto.DriverLicenseNumber))
                .ReturnsAsync(new DeliveryPerson());

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                _deliveryPersonService.RegisterDeliveryPersonAsync(deliveryPersonDto));
            Assert.Equal("A delivery person with this Driver License Number already exists.", exception.Message);
        }

        [Fact]
        public async Task UpdateDriverLicenseImageAsync_ShouldThrowException_WhenDeliveryPersonNotFound()
        {
            // Arrange
            var deliveryPersonId = Guid.NewGuid();
            var driverLicenseImageBase64 = "newBase64String";

            _deliveryPersonRepositoryMock
                .Setup(repo => repo.GetByIdAsync(deliveryPersonId))
                .ReturnsAsync((DeliveryPerson)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                _deliveryPersonService.UpdateDriverLicenseImageAsync(deliveryPersonId, driverLicenseImageBase64));
            Assert.Equal("Delivery person not found", exception.Message);
        }

        [Fact]
        public async Task UpdateDriverLicenseImageAsync_ShouldUpdateDriverLicenseImage_WhenDeliveryPersonExists()
        {
            // Arrange
            var deliveryPersonId = Guid.NewGuid();
            var driverLicenseImageBase64 = "newBase64String";

            var existingDeliveryPerson = new DeliveryPerson
            {
                Id = deliveryPersonId,
                DriverLicenseImageBase64 = "oldBase64String"
            };

            _deliveryPersonRepositoryMock
                .Setup(repo => repo.GetByIdAsync(deliveryPersonId))
                .ReturnsAsync(existingDeliveryPerson);

            // Act
            await _deliveryPersonService.UpdateDriverLicenseImageAsync(deliveryPersonId, driverLicenseImageBase64);

            // Assert
            _deliveryPersonRepositoryMock.Verify(repo => repo.UpdateAsync(It.Is<DeliveryPerson>(dp =>
                dp.Id == deliveryPersonId &&
                dp.DriverLicenseImageBase64 == driverLicenseImageBase64
            )), Times.Once);
        }
    }
}
