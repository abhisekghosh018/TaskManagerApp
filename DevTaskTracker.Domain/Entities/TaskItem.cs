using DevTaskTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTaskTracker.Domain.Entities
{
    public class TaskItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; }

        // Task status like Open, InProgress, Done
        public TaskStatusEnum Status { get; set; }

        // Foreign Key - Assigned User
        public string? AssignedToUserId { get; set; }
        public AppUser? AssignedToUser { get; set; }
    }
}
