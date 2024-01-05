using Microsoft.AspNetCore.Mvc;

namespace PspPos.Models
{
    public class SamplePostModel
    {
        public string? Location { set; get; }
        public DateTime Date { set; get; } = DateTime.Now;

    }
}