using Microsoft.AspNetCore.Mvc;

namespace PspPos.Models
{
    public class ItemViewModel
    {
        public Guid Id { set; get; }
        public string? Name { set; get; }
        public string? Description { set; get; }
        public double? Price { set; get; }
        public double? Tax { set; get; }

    }
}