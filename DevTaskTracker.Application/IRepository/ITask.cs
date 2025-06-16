using DevTaskTracker.Application.DTOs.Common;
using DevTaskTracker.Application.DTOs.TaskDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DevTaskTracker.Application.Interfaces
{
    public interface ITask
    {
        Task<CommonReturnDto>GetTasksAsync(ClaimsPrincipal user, int pageNum);
        Task<CommonReturnDto> CreateTaskAsync(CreateTaskItemDto dto);
        Task<CommonReturnDto> FilterTasksByIdStstusprioritydueDateAsync(string? priority, string? status, DateTime? dueDate);
        Task<CommonReturnDto> UpdateTaskStatusAsync(UpdateStatusDto dto);
    }
}
