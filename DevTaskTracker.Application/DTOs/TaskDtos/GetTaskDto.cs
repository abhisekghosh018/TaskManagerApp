using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTaskTracker.Application.DTOs.TaskDtos
{
    public class GetTaskDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } 
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public string Status { get; set; } 
        public string Priority { get; set; }        
        public string MemberName { get; set; }
    }
}
