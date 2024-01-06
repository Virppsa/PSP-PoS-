namespace PspPos.Models
{
    public class Appointment 
    { 
        public Guid ServiceId { get; set; }
        public Guid StoreId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid WorkerId { get; set; }
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
    }
}
