using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTaskTracker.Application.DTOs.MemberDtos
{
    public class CreateMemberDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required, EmailAddress]
        public string WorkEmail { get; set; }
        [Required]
        
        public string? Password { get; set; }
        [Required]
        public string Role { get; set; }
        public string? GitRepo { get; set; }
        public string? IP { get; set; }
        [Required]
        public string OrganizationId { get; set; }
        public string UserId { get; set; }

    }
}
