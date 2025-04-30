using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTaskTracker.Application.DTOs.TaskDtos
{
    public class TaskItemDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? AssignedToUserId { get; set; }
    }
}
