using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTaskTracker.Domain.Entities
{
    public class Member
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]  
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]  
        public string WorkEmail { get; set; }
        [Required]  
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string? IP { get; set; }
        public string? GitRepo { get; set; }
        public string? Status { get; set; }
        

        [Required]
        public string OrganizationId { get; set; }
        public Organization Organization { get; set; }

        // Navigation properties
        public virtual ICollection<TaskItem> AssignedTasks { get; set; } = new List<TaskItem>();

    }
}
