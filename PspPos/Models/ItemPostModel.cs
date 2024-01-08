using Microsoft.AspNetCore.Mvc;

namespace PspPos.Models
{
    public class ItemPostModel
    {
        public string? Name { set; get; }
        public string? Description { set; get; }
        public double? Price { set; get; }
    }
}