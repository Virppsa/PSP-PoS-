using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PspPos.Models
{
    public class InventoryViewModel
    {
        public Guid Id { set; get; }
        public Guid StoreId { set; get; }
        public Guid ItemId { set; get; }
        public int Amount { set; get; }
        public int LowStockThreshold { set; get; }
    }
}