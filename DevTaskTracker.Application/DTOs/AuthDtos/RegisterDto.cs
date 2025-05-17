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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        public string? Contact { get; set; }
        [Required]
        public string OrganizationName { get; set; } // Name of the organization           
        [Required]
        [MinLength(3), MaxLength(16)]
        public string UserName { get; set; }
        [Required]
        [MinLength(8),MaxLength(16)]
        public string Password { get; set; }
      
    }
}
