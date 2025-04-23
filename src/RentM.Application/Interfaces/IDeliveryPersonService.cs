using RentM.Application.DTOs;

namespace RentM.Application.Interfaces
{
    public interface IDeliveryPersonService
    {
        Task RegisterDeliveryPersonAsync(DeliveryPersonDto deliveryPersonDto);
        Task UpdateDriverLicenseImageAsync(Guid deliveryPersonId, string driverLicenseImageUrl);
    }
}

