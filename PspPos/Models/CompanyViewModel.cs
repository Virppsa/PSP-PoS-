using Microsoft.AspNetCore.Mvc;

namespace PspPos.Models
{
    public class CompanyViewModel
    {
        public Guid Id { set; get; }
        public string? Name { set; get; }
        public string? Email { set; get; }

    }
}