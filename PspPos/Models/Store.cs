using PspPos.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PspPos.Models
{
    public class Store
    {
        [Key]
        public Guid Guid { get; set; }
        [ForeignKey("Company")]
        public Guid CompanyId { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }

        public virtual Company Company { get; set; }
    }
}
