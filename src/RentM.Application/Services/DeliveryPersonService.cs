using RentM.Application.DTOs;
using RentM.Application.Interfaces;
using RentM.Domain.Models;
using RentM.Infrastructure.Interfaces;

public class DeliveryPersonService : IDeliveryPersonService
{
    private readonly IDeliveryPersonRepository _deliveryPersonRepository;

    public DeliveryPersonService(IDeliveryPersonRepository deliveryPersonRepository)
    {
        _deliveryPersonRepository = deliveryPersonRepository;
    }

    public async Task RegisterDeliveryPersonAsync(DeliveryPersonDto deliveryPersonDto)
    {
        // 1. Validate CNH type
        var validCnhTypes = new[] { "A", "B", "A+B" };
        if (!validCnhTypes.Contains(deliveryPersonDto.DriverLicenseType))
            throw new Exception("Invalid CNH type. Valid types are: A, B, or A+B.");

        // 2. Validate unique CNPJ
        var existingByCnpj = await _deliveryPersonRepository.GetByCnpjAsync(deliveryPersonDto.Cnpj);
        if (existingByCnpj != null)
            throw new Exception("A delivery person with this CNPJ already exists.");

        // 3. Validate unique Driver License Number
        var existingByDriverLicense = await _deliveryPersonRepository.GetByDriverLicenseNumberAsync(deliveryPersonDto.DriverLicenseNumber);
        if (existingByDriverLicense != null)
            throw new Exception("A delivery person with this Driver License Number already exists.");

        // 4. Create the delivery person
        var deliveryPerson = new DeliveryPerson
        {
            Id = Guid.NewGuid(),
            Name = deliveryPersonDto.Name,
            Cnpj = deliveryPersonDto.Cnpj,
            BirthDate = deliveryPersonDto.BirthDate,
            DriverLicenseNumber = deliveryPersonDto.DriverLicenseNumber,
            DriverLicenseType = deliveryPersonDto.DriverLicenseType,
            DriverLicenseImageBase64 = deliveryPersonDto.DriverLicenseImageBase64
        };
    }

    public async Task UpdateDriverLicenseImageAsync(Guid deliveryPersonId, string driverLicenseImageBase64)
    {
        var deliveryPerson = await _deliveryPersonRepository.GetByIdAsync(deliveryPersonId);
        if (deliveryPerson == null) throw new Exception("Delivery person not found");

        deliveryPerson.DriverLicenseImageBase64 = driverLicenseImageBase64; // Base64
        await _deliveryPersonRepository.UpdateAsync(deliveryPerson);
    }
}
