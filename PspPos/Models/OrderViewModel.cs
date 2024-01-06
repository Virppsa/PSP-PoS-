namespace PspPos.Models;

public class OrderViewModel
{
    public int Id { set; get; }
    public int WorkerId { set; get; }
    public int CustomerId { set; get; }
    public int PaymentMethodId { set; get; }
    public double Gratuity { set; get; }
    public double Tax { set; get; }
    public double TotalAmount { set; get; }
    public int[] Appointments { set; get; } = new int[0];
    public int[] ItemOrders { set; get; } = new int[0];
    public DateTime OrderDate { set; get; } = DateTime.Now;
    public string Status { set; get; } = "Placed";
}