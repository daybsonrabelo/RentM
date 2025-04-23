using Microsoft.AspNetCore.Mvc;
using RentM.Application.DTOs;
using RentM.Application.Interfaces;

[ApiController]
[Route("api/delivery-persons")]
public class DeliveryPersonController : ControllerBase
{
    private readonly IDeliveryPersonService _deliveryPersonService;

    public DeliveryPersonController(IDeliveryPersonService deliveryPersonService)
    {
        _deliveryPersonService = deliveryPersonService;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterDeliveryPerson([FromBody] DeliveryPersonDto deliveryPersonDto)
    {
        await _deliveryPersonService.RegisterDeliveryPersonAsync(deliveryPersonDto);
        return Ok();
    }

    [HttpPut("{id}/driver-license-image")]
    public async Task<IActionResult> UpdateDriverLicenseImage(Guid id, [FromBody] string driverLicenseImageBase64)
    {
        await _deliveryPersonService.UpdateDriverLicenseImageAsync(id, driverLicenseImageBase64);
        return Ok();
    }
}