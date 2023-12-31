using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PspPos.Models
{
    public class OrderItemPostModel
    {
        public Guid ItemId { set; get; }
        public Guid StoreId { set; get; }
        public List<Guid>? ItemOptions { set; get; }
        public string? Status { set; get; }
        public Guid? WorkerId { set; get; }
    }
}