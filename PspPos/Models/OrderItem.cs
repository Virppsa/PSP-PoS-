using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PspPos.Models
{
    public class OrderItem
    {
        public Guid Id { set; get; }
        public Guid CompanyId { set; get; }
        public Guid ItemId { set; get; }
        public Guid StoreId { set; get; }
        public Guid? OrderId { set; get; } = null;
        public List<Guid>? ItemOptions { set; get; }

        public string? Status { set; get; } = "Placed";
        public Guid WorkerId { set; get; }
    }
}