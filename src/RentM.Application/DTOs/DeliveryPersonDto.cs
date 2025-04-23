namespace RentM.Application.DTOs
{
    public class DeliveryPersonDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Cnpj { get; set; }
        public string DriverLicenseNumber { get; set; }
        public string DriverLicenseType { get; set; }
        public DateTime BirthDate { get; set; }
        public string DriverLicenseImageBase64 { get; set; }

    }
}

