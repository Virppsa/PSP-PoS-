namespace PspPos.Models.DTO.Requests
{
    public class GetAppointmentsRequest
    {
        public Guid? ServiceId { get; set;}
        public string? LowerDateBoundary { get; set;}
        public string? HigherDateBoundary { get; set;}
    }
}
