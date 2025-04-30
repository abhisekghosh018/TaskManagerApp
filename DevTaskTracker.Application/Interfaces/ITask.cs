using DevTaskTracker.Application.DTOs.Common;
using DevTaskTracker.Application.DTOs.TaskDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTaskTracker.Application.Interfaces
{
    public interface ITask
    {
        Task<IEnumerable<CommonReturnDto>> GetTasksAsync();

        Task<CommonReturnDto> CreateTaskAsync(TaskItemDto dto);
    }
}
