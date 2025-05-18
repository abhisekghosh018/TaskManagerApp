using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTaskTracker.Application.DTOs.MemberDtos
{
    public class UpdateMemberDto
    {
        public Guid Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }       
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }
        public string Email { get; set; }
        public string? GitRepo { get; set; }
        public string? IP { get; set; }
    }
}
