namespace PspPos.Models;

public class PaymentPostModel
{
    public Guid OrderId { get; set; }
    public string PaymentMethod { get; set; }
    public string PaymentStatus { get; set; }
}