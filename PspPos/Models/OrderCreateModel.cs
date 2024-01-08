namespace PspPos.Models;

public class OrderCreateModel
{
    public Guid? WorkerId { set; get; }
    public Guid CustomerId { set; get; }
    public int PaymentMethodId { set; get; }
    public string Status { set; get; } = "Placed";
}