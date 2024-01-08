using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PspPos.Models
{
    public class Company
    {
        [System.ComponentModel.DataAnnotations.Key]
        public Guid Id { set; get; }
        public string? Name { set; get; }
        public string? Email { set; get; }

    }
}