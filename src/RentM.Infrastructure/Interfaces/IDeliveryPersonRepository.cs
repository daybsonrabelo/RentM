using RentM.Domain.Models;

namespace RentM.Infrastructure.Interfaces
{
    public interface IDeliveryPersonRepository
    {
        Task AddAsync(DeliveryPerson deliveryPerson);
        Task<DeliveryPerson> GetByIdAsync(Guid id);
        Task<DeliveryPerson> GetByCnpjAsync(string cnpj); // Novo m�todo
        Task<DeliveryPerson> GetByDriverLicenseNumberAsync(string driverLicenseNumber); // Novo m�todo
        Task UpdateAsync(DeliveryPerson deliveryPerson);

    }
}

