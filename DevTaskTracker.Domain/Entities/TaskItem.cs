using DevTaskTracker.Domain.Enums;

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
        public TaskPriorityEnum Priority { get; set; }

        // Foreign Key - Assigned User        
        public string? MemberId { get; set; }
        public Member? Member { get; set; }
    }
}
