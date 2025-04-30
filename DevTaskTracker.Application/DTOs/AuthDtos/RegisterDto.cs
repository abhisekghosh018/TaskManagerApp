using DevTaskTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTaskTracker.Application.DTOs.AuthDtos
{
    public class RegisterDto
    {
        [Required]
        public string FullName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        public string? Contact { get; set; }
        public string? OrganizationName { get; set; } // Name of the organization
        public string? PersonName { get; set; }       
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
      
    }
}
