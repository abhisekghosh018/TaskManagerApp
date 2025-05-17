using DevTaskTracker.Domain.Enums;

namespace DevTaskTracker.Domain.Entities
{
    public class TaskItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CompletedAt { get; set; }
        public TaskStatusEnum Status { get; set; }
        public TaskPriorityEnum Priority { get; set; }
        public double? EstimatedHours { get; set; }
        public double? SpentHours { get; set; }

        // Who is this task assigned to?
        public Guid MemberId { get; set; }
        public Member Member { get; set; }

        // Who assigned this task? (Typically an AppUser)
        public string AssignedByUserId { get; set; }
        public AppUser AssignedBy { get; set; }

        public string OrganizationId { get; set; }
        public Organization Organization { get; set; }

        // Optional: Tracking who last updated
        public string LastUpdatedByUserId { get; set; }
        public AppUser? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedAt { get; set; }

    }


}
