namespace PspPos.Models;

public class OrderPostModel
{
    public int CustomerId { set; get; } // TODO: they forgot to mention customerId anywhere else
    public int PaymentMethodId { set; get; }
    public double Gratuity { set; get; }
    public int[] Appointments { set; get; } = new int[0];
    public int[] ItemOrders { set; get; } = new int[0];
}