namespace PspPos.Models.DTO.Requests
{
    public class AppointmentCreateRequest
    {
        public Guid ServiceId { get; set; }
        public Guid WorkerId { get; set; }
        public required string StartDate { get; set; }
        public required string EndDate { get; set; }
        public Guid StoreId { get; set; }
        public Guid? OrderId { get; set; }
    }
}
