using DevTaskTracker.Application.DTOs.Common;
using DevTaskTracker.Application.DTOs.TaskDtos;
using DevTaskTracker.Application.Interfaces;
using DevTaskTracker.Domain.Entities;
using DevTaskTracker.Infrastructure.Common;
using DevTaskTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;



namespace DevTaskTracker.Infrastructure.Services
{
    public class TaskService : ITask
    {
        private readonly AppDbContext _appDbContext;
        private readonly ICommonImplementations<TaskItem> _commonImplementations;
        private IConfiguration _config;
        public TaskService(AppDbContext appDbContext, 
            ICommonImplementations<TaskItem> commonImplementations, IConfiguration config)
        {
            _appDbContext = appDbContext;
            _commonImplementations = commonImplementations;
            _config = config;
        }
        /// <summary>
        /// /// Create a new task item and save it to the database.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<CommonReturnDto> CreateTaskAsync(CreateTaskItemDto dto)
        {
            var task = new TaskItem
            {
                Title = dto.Title ?? "",
                Description = dto.Description,
                MemberId = dto.AssignedToMemberId,
                DueDate = dto.DueDate,
                Status = dto.Status,
                Priority = dto.Priority,
                AssignedByUserId = dto.AssignedByUserId,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedByUserId = dto.AssignedByUserId,
                OrganizationId = dto.OrganizationId
            };

            _appDbContext.TaskItems.Add(task);
            await _appDbContext.SaveChangesAsync();

            return new CommonReturnDto
            {
                IsSuccess = true,
                SuccessMessage = CommonAlerts.TaskCreateSuccess,
                Data = task.Id
            };
        }
        /// <summary>
        /// Fetch tasks with member details based on user role.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pageNum"
        /// <returns></returns>
        public async Task<CommonReturnDto> GetTasksAsync(ClaimsPrincipal user, int pageNum)
        {

            int pageSize = Convert.ToInt32(_config["pagination:PageSize"]);
            var userRole = user.FindFirst(ClaimTypes.Role)?.Value;

            var query = _commonImplementations.GetQueryableIncluding(t=> t.Member);
            var memberId = query.Select(m => m.MemberId).FirstOrDefault();
            if (userRole == "User")
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                query = query.Where(t => t.AssignedByUserId == userId || t.MemberId == memberId);
            }
            else
            {
                var orgAdminId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                query = query.Where(t => t.AssignedByUserId == orgAdminId);
            }
            var totalCount = await query.CountAsync();
            var pagedTasks = await _commonImplementations.Pagination(query, pageNum);
                

            if (userRole == "User")
            {
                var result =pagedTasks.Select(t => new GetTaskDto
                {
                    Title = t.Title,
                    Description = t.Description,
                    MemberName = t.Member?.FirstName + " " + t.Member?.LastName,
                    DueDate = t.DueDate,
                    Status = t.Status.ToString(),
                    Priority = t.Priority.ToString(),
                    //TotalCount = totalCount,
                }).ToList();

                if (!result.Any())
                {
                    return new CommonReturnDto
                    {
                        IsSuccess = false,
                        ErrorMessage = CommonAlerts.TaskCreateFailed
                    };
                }

                return new CommonReturnDto
                {
                    IsSuccess = true,
                    Data = result
                };
            }
            else
            {
                var result = pagedTasks.Select(t => new GetTaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    MemberName = t.Member?.FirstName + " " + t.Member?.LastName,
                    DueDate = t.DueDate,
                    Status = t.Status.ToString(),
                    Priority = t.Priority.ToString(),
                    CreatedAt = t.CreatedAt,
                    LastUpdatedAt = t.LastUpdatedAt,
                    LastUpdatedByUserId = t.LastUpdatedByUserId,
                    AssignedByUserId = t.AssignedByUserId,
                    EstimatedHours = t.EstimatedHours ?? 0,
                    SpentHours = t.SpentHours ?? 0,
                    //TotalCount = totalCount,
                }).ToList();

                return new CommonReturnDto
                {
                    IsSuccess = true,
                    Data = result
                };
            }
        }

        /// <summary>
        /// Filter tasks by ID, status, priority, and due date.
        /// </summary>    
        /// <param name="status"></param>
        /// <param name="priority"></param>
        /// <param name="dueDate"></param>
        /// <returns></returns>
        public async Task<CommonReturnDto> FilterTasksByIdStstusprioritydueDateAsync(
        string? priority, string? status, DateTime? dueDate)
        {
            var query = _commonImplementations.GetIQueryTaskItem();

            if (!string.IsNullOrEmpty(priority))
            {
                query = query.Where(t => t.Priority.ToString() == priority);
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(t => t.Status.ToString() == status);
            }

            if (dueDate.HasValue)
            {
                query = query.Where(t => t.DueDate == dueDate);
            }

            var result = await query.ToListAsync();

            if (result.Count == 0)
            {
                return new CommonReturnDto
                {
                    IsSuccess = false,
                    ErrorMessage = CommonAlerts.TaskNotFound
                };
            }

            return new CommonReturnDto
            {
                IsSuccess = true,
                Data = result
            };
        }

    }

}
