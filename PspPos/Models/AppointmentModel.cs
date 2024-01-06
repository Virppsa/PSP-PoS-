namespace PspPos.Models
{
    public class Appointment 
    { 
        public int ServiceId { get; set; }
        public int StoreId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int WorkerId { get; set; }
        public int Id { get; set; }
        public int OrderId { get; set; }
    }
}
