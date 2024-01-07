using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using PspPos.Models.Enums;
using System.Data;

namespace PspPos.Models
{
    public class User
    {
        [Key]
        public Guid GUID { get; set; }
        public Guid CompanyId { get; set; }
        public Guid? StoreId { get; set; }
        public string? Role { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public int? LoyaltyPoints { get; set; }

        //public virtual Company Company { get; set; }

        [NotMapped] // This property is not mapped to the database
        public UserRoles UserRoles
        {
            get => Enum.TryParse<UserRoles>(Role, out var roleEnum) ? roleEnum : default;
            set => Role = value.ToString();
        }
    }

}
