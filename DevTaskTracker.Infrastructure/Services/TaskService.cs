using DevTaskTracker.Application.DTOs.Common;
using DevTaskTracker.Application.DTOs.TaskDtos;
using DevTaskTracker.Application.Interfaces;
using DevTaskTracker.Domain.Entities;
using DevTaskTracker.Infrastructure.Common;
using DevTaskTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DevTaskTracker.Infrastructure.Services
{
    public class TaskService : ITask
    {
        private readonly AppDbContext _appDbContext;
        private readonly ICommonImplementations _commonImplementations;  
        public TaskService(AppDbContext appDbContext, ICommonImplementations commonImplementations)
        {
            _appDbContext = appDbContext;
            _commonImplementations = commonImplementations;
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
                LastUpdatedByUserId= dto.AssignedByUserId,
                OrganizationId =dto.OrganizationId
            };

            _appDbContext.TaskItems.Add(task);
            await _appDbContext.SaveChangesAsync();

            return new CommonReturnDto
            {
                IsSuccess = true,
                SuccessMessage =CommonAlerts.TaskCreateSuccess,
                Data = task.Id
            };
        }
        /// <summary>
        /// Fetch tasks with member details based on user role.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<CommonReturnDto> GetTasksAsync(ClaimsPrincipal user)
        {
            var userRole = user.FindFirst(ClaimTypes.Role)?.Value;

            // Fetch tasks with member details
            var tasks = await _appDbContext.TaskItems
                .Include(t => t.Member)
                .ToListAsync();

            if (userRole == "User")
            {
                // Filter or map tasks as needed
                var result = tasks.Select(t => new GetTaskDto
                {
                    Title = t.Title,
                    Description = t.Description,
                    MemberName = t.Member?.FirstName + " " + t.Member?.LastName,
                    DueDate = t.DueDate,
                    Status = t.Status.ToString(), // Handle null status
                    Priority = t.Priority.ToString()  // Handle null priority
                }).ToList();

                if (result.Count == 0)
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

            return new CommonReturnDto
            {
                IsSuccess = false,
                ErrorMessage = "Unauthorized role."
            };
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
