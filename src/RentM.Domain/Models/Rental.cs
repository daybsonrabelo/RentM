namespace RentM.Domain.Models
{
    public class Rental
    {
        public Guid Id { get; set; }
        public Guid MotorcycleId { get; set; }
        public Guid DeliveryPersonId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpectedEndDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public decimal TotalCost { get; set; }
    }
}
