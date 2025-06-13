using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTaskTracker.Application.DTOs.TaskDtos
{
    public class UpdateStatusDto
    {
        public Guid Id { get; set; }
        public Guid MemberId { get; set; }
        public string Status { get; set; }
    }
}
