using DevTaskTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTaskTracker.Application.DTOs.TaskDtos
{
    public class CreateTaskItemDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        [Required]
        public string? AssignedToMemberId { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        public TaskStatusEnum Status { get; set; } = TaskStatusEnum.Open;
        [Required]
        public TaskPriorityEnum Priority { get; set; }  =TaskPriorityEnum.Medium;
    }
}
