namespace PspPos.Models
{
    public class ServiceDiscount
    {
        public Guid ServiceId { get; set; }
        public int DiscountPercentage { get; set; }
        public DateTime ValidUntil { get; set; }
        public string? Conditions { get; set; }
    }
}
