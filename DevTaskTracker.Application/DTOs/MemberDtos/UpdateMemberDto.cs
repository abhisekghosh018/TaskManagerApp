using System.ComponentModel.DataAnnotations;
using System.Data;

namespace DevTaskTracker.Application.DTOs.MemberDtos
{
    public class UpdateMemberDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        //public string? Status { get; set; }       
        public string Role { get; set; }
        public string Email { get; set; }
        public string? GitRepo { get; set; }      

        public bool IsActive { get; set; }
        //public string? IP { get; set; }       
        public string RowVersion { get; set; }
    }
}
