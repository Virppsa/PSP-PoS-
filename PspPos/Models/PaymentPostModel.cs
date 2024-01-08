namespace PspPos.Models;

public class PaymentPostModel
{
    public Guid CustomerId { get; set; }
    public Guid OrderId { get; set; }
    public string PaymentMethod { get; set; }
    public int? LoyaltyPointsToUse { get; set; }
    public float? TotalAmount { get; set; }
}