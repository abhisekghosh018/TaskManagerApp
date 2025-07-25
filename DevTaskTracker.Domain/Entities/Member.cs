﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTaskTracker.Domain.Entities
{
    public class Member
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]  
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]  
        public string WorkEmail { get; set; } // Act as user name        
        [Required]  
        public string Role { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string? IP { get; set; }
        public string? GitRepo { get; set; }
        public string? Status { get; set; }
        public bool IsActive { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        
        [Required]
        public string OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        // Navigation properties
        public virtual ICollection<TaskItem> AssignedTasks { get; set; } = new List<TaskItem>();

    }
}
