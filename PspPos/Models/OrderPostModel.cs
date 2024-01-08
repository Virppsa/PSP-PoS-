namespace PspPos.Models;

public class OrderPostModel
{
    public Guid? WorkerId { set; get; }
    public Guid CustomerId { set; get; }
    public int PaymentMethodId { set; get; }
    public Guid[] Appointments { set; get; } = new Guid[0];
    public Guid[] ItemOrders { set; get; } = new Guid[0];
    public string Status { set; get; } = "Placed";
}