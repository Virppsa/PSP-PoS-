using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PspPos.Models
{
    public class Item
    {
        public Guid Id { set; get; }
        public Guid CompanyId { set; get; }
        public string? Name { set; get; }
        public string? Description { set; get; }
        public double? Price { set; get; }
        public double? Tax { set; get; }
        public string? SerializedDiscount { get; set; }
    }
}