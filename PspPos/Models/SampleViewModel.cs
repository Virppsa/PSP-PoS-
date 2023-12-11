using Microsoft.AspNetCore.Mvc;

namespace PspPos.Models         
{
    public class SampleViewModel
    {
        public int Id { set; get; }
        public string? Location { set; get; }
        public DateTime? Date { set; get; } = DateTime.Now;

    }
}