using RentM.Application.DTOs;
using RentM.Application.Interfaces;
using RentM.Domain.Models;
using RentM.Infrastructure.Interfaces;

namespace RentM.Application.Services
{
    public class MotorcycleService : IMotorcycleService
    {
        private readonly IMotorcycleRepository _motorcycleRepository;

        public MotorcycleService(IMotorcycleRepository motorcycleRepository)
        {
            _motorcycleRepository = motorcycleRepository;
        }

        public async Task RegisterMotorcycleAsync(MotorcycleDto motorcycleDto)
        {
            var motorcycle = new Motorcycle
            {
                Id = Guid.NewGuid(),
                Year = motorcycleDto.Year,
                Model = motorcycleDto.Model,
                LicensePlate = motorcycleDto.LicensePlate
            };

            await _motorcycleRepository.AddAsync(motorcycle);
        }

        public async Task<IEnumerable<MotorcycleDto>> GetMotorcyclesAsync(string licensePlate)
        {
            var motorcycles = await _motorcycleRepository.GetByLicensePlateAsync(licensePlate);
            return motorcycles.Select(m => new MotorcycleDto
            {
                Id = m.Id,
                Year = m.Year,
                Model = m.Model,
                LicensePlate = m.LicensePlate
            });
        }

        public async Task UpdateLicensePlateAsync(Guid motorcycleId, string newLicensePlate)
        {
            var motorcycle = await _motorcycleRepository.GetByIdAsync(motorcycleId);
            if (motorcycle == null) throw new Exception("Motorcycle not found");

            motorcycle.LicensePlate = newLicensePlate;
            await _motorcycleRepository.UpdateAsync(motorcycle);
        }

        public async Task RemoveMotorcycleAsync(Guid motorcycleId)
        {
            var motorcycle = await _motorcycleRepository.GetByIdAsync(motorcycleId);
            if (motorcycle == null) throw new Exception("Motorcycle not found");

            await _motorcycleRepository.DeleteAsync(motorcycleId);
        }
    }
}

