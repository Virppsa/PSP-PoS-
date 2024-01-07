using System.ComponentModel.DataAnnotations;

namespace PspPos.Models.DTO.Requests
{
    public class UserLoyaltyUpdateRequest
    {
        [Required]
        public int LoyaltyPoints { get; set; }
    }
}
