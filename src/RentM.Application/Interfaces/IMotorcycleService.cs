using RentM.Application.DTOs;

namespace RentM.Application.Interfaces
{
    public interface IMotorcycleService
    {
        Task RegisterMotorcycleAsync(MotorcycleDto motorcycleDto);
        Task<IEnumerable<MotorcycleDto>> GetMotorcyclesAsync(string licensePlate);
        Task UpdateLicensePlateAsync(Guid motorcycleId, string newLicensePlate);
        Task RemoveMotorcycleAsync(Guid motorcycleId);
    }
}

