using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PspPos.Models
{
    public class User
    {
        [Key]
        public Guid GUID { get; set; }
        [ForeignKey("Company")]
        public Guid CompanyId { get; set; }
        public int StoreId { get; set; }
        public string? Role { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public int? LoyaltyPoints { get; set; }

        public virtual Company Company { get; set; }
    }

}
