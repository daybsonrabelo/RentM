using Microsoft.AspNetCore.Mvc;
using RentM.Application.DTOs;
using RentM.Application.Interfaces;
using RentM.Domain.Models;
using RentM.Infrastructure.Messaging;

[ApiController]
[Route("api/motorcycles")]
public class MotorcycleController : ControllerBase
{
    private readonly IMotorcycleService _motorcycleService;
    private readonly MotorcycleEventPublisher _motorcycleEventPublisher;

    public MotorcycleController(IMotorcycleService motorcycleService, MotorcycleEventPublisher motorcycleEventPublisher)
    {
        _motorcycleService = motorcycleService;
        _motorcycleEventPublisher = motorcycleEventPublisher;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterMotorcycle([FromBody] MotorcycleDto motorcycleDto)
    {
        var motorcycle = new Motorcycle
        {
            Id = Guid.NewGuid(),
            Year = motorcycleDto.Year,
            Model = motorcycleDto.Model,
            LicensePlate = motorcycleDto.LicensePlate
        };

        await _motorcycleService.RegisterMotorcycleAsync(motorcycleDto);

        if (motorcycle.Year == 2024)
        {
            // Publish event
            await _motorcycleEventPublisher.PublishMotorcycleRegisteredEventAsync(motorcycle);
        }

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetMotorcycles([FromQuery] string licensePlate)
    {
        var motorcycles = await _motorcycleService.GetMotorcyclesAsync(licensePlate);
        return Ok(motorcycles);
    }
}

