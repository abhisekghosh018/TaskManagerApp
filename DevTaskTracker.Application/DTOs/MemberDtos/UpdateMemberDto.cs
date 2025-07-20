using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
        public string Role { get; set; }
        [Required]
        public string WorkEmail { get; set; }
        public string? GitRepo { get; set; }      
        public bool IsActive { get; set; }
        public string RowVersion { get; set; }
        public string? ImageUrl {  get; set; }

        public IFormFile? File { get; set; } 
    }
}
