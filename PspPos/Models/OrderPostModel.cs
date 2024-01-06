namespace PspPos.Models;

public class OrderPostModel
{
    public int WorkerId { set; get; }
    public int CustomerId { set; get; }
    public int PaymentMethodId { set; get; }
    public double Gratuity { set; get; }
    public int[] Appointments { set; get; } = new int[0];
    public int[] ItemOrders { set; get; } = new int[0];
    public string Status = "Placed";
}