using System.ComponentModel.DataAnnotations;

namespace PspPos.Models.DTO.Requests
{
    public class AppointmentCreateRequest
    {
        [Required]
        public Guid ServiceId { get; set; }
        public Guid? WorkerId { get; set; }
        [Required]
        public required string StartDate { get; set; }
        [Required]
        public required string EndDate { get; set; }
        public Guid? StoreId { get; set; }
        public Guid? OrderId { get; set; }
    }
}
