using Microsoft.AspNetCore.Mvc;

namespace PspPos.Models
{
    public class CompanyViewModel
    {
        public int Id { set; get; }
        public string? Name { set; get; }
        public string? Email { set; get; }

    }
}