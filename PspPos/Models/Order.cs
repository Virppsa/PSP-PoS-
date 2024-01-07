namespace PspPos.Models;

public class Order
{
    public Guid CompanyId { get; set; }
    public Guid Id { set; get; }
    public Guid WorkerId { set; get; }
    public Guid CustomerId { set; get; }
    public int PaymentMethodId { set; get; }
    public double Gratuity { set; get; }
    public double Tax { set; get; }
    public double TotalAmount { set; get; }
    public Guid[] Appointments { set; get; } = new Guid[0];
    public Guid[] ItemOrders { set; get; } = new Guid[0];
    public DateTime OrderDate { set; get; } = DateTime.Now;
    public string Status = "Placed";
}