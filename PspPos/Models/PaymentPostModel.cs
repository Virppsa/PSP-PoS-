namespace PspPos.Models;

public class PaymentPostModel
{
    public Guid CustomerId { get; set; }
    public Guid OrderId { get; set; }
    public string PaymentMethod { get; set; } = "Card";
    public int LoyaltyPointsToUse { get; set; } = 0;
    public double AmountPaid { get; set; }
    public double Gratuity { get; set; }
}