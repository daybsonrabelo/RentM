using RentM.Domain.Models;

namespace RentM.Infrastructure.Interfaces
{
    public interface IDeliveryPersonRepository
    {
        Task AddAsync(DeliveryPerson deliveryPerson);
        Task<DeliveryPerson> GetByIdAsync(Guid id);
        Task<DeliveryPerson> GetByCnpjAsync(string cnpj); // Novo método
        Task<DeliveryPerson> GetByDriverLicenseNumberAsync(string driverLicenseNumber); // Novo método
        Task UpdateAsync(DeliveryPerson deliveryPerson);

    }
}

