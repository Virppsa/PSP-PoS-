using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PspPos.Models
{
    public class ItemOptionPostModel
    {
        public Guid ItemId { set; get; }
        public string? Name { set; get; }
        public double? Price { set; get; }
    }
}