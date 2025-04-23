using Microsoft.AspNetCore.Mvc;
using RentM.Application.Interfaces;
using RentM.Application.Requests;
using RentM.Domain.ValueObjects;

[ApiController]
[Route("api/rentals")]
public class RentalController : ControllerBase
{
    private readonly IRentalService _rentalService;

    public RentalController(IRentalService rentalService)
    {
        _rentalService = rentalService;
    }

    [HttpPost]
    public async Task<IActionResult> RentMotorcycle([FromBody] RentMotorcycleRequest request)
    {
        await _rentalService.RentMotorcycleAsync(request.DeliveryPersonId, request.MotorcycleId, request.Plan, request.StartDate);
        return Ok();
    }

    [HttpGet("{id}/cost")]
    public async Task<IActionResult> CalculateRentalCost(Guid id, [FromQuery] DateTime returnDate)
    {
        var cost = await _rentalService.CalculateRentalCostAsync(id, returnDate);
        return Ok(cost);
    }
}
