using RentM.Domain.ValueObjects;

namespace RentM.Application.Requests
{
    public class RentMotorcycleRequest
    {
        public Guid DeliveryPersonId { get; set; }
        public Guid MotorcycleId { get; set; }
        public RentalPlan Plan { get; set; }
        public DateTime StartDate { get; set; }
    }
}
