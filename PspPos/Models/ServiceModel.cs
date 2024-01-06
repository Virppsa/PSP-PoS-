namespace PspPos.Models
{
    public class Service
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public Guid Id { get; set; }
        public double Tax { get; set; }
        public Guid companyId { get; set; }
        public string SerializedDiscount { get; set; }

    }
}
