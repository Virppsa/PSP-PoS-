namespace PspPos.Models;

public class Payment
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }
    public string PaymentMethod { get; set; }
    public string PaymentStatus { get; set; }
    public float? LoyaltyDiscount { get; set; }
    public float? TotalAmount { get; set; }
}