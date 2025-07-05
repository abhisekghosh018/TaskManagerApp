using AutoMapper;
using DevTaskTracker.Application.DTOs.Common;
using DevTaskTracker.Application.DTOs.TaskDtos;
using DevTaskTracker.Application.Interfaces;
using DevTaskTracker.Domain.Entities;
using DevTaskTracker.Domain.Enums;
using DevTaskTracker.Infrastructure.Common;
using DevTaskTracker.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHttpContextAccessor _iHttpContext;
        private readonly IMapper _iMaper;

        public TaskService(AppDbContext appDbContext,
            ICommonImplementations<TaskItem> commonImplementations, IConfiguration config,
            RoleManager<IdentityRole> roleManager, IHttpContextAccessor iHttpContext, IMapper iMaper)
        {
            _appDbContext = appDbContext;
            _commonImplementations = commonImplementations;
            _config = config;
            _roleManager = roleManager;
            _iHttpContext = iHttpContext;
            _iMaper = iMaper;
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

            var query = _commonImplementations.GetQueryableIncluding(t => t.Member);

            var totalCount = await query.CountAsync();
            var pagedTasks = await _commonImplementations.Pagination(query, pageNum);

            if (userRole == "User")
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var member = _appDbContext.Members.FirstOrDefault(m => m.AppUserId == userId);
                if (member != null)
                {
                    pagedTasks = pagedTasks.Where(m => m.MemberId == member.Id).ToList();
                }

                totalCount = pagedTasks.Count();

                var result = pagedTasks.Select(t => new GetTaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    MemberName = t.Member?.FirstName + " " + t.Member?.LastName,
                    DueDate = t.DueDate,
                    Status = t.Status.ToString(),
                    Priority = t.Priority.ToString(),
                    TotalCount = totalCount,

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
                    TotalCount = totalCount,

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
        /// <summary>
        /// Update task status 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<CommonReturnDto> UpdateTaskStatusAsync(UpdateStatusDto dto)
        {
            var user = _iHttpContext.HttpContext.User;
            var userRole = user?.FindFirst(ClaimTypes.Role)?.Value;

            string? taskStatus = null;
           
            if (userRole != null && userRole == "User")
            {
                if (dto.Status == TaskStatusEnum.Open.ToString())
                {
                    taskStatus = TaskStatusEnum.InProgress.ToString();
                }
                else if (dto.Status == TaskStatusEnum.InProgress.ToString())
                {
                    taskStatus = TaskStatusEnum.InReview.ToString();
                }
            }
            else // Admin or other roles
            {
                if (dto.Status == TaskStatusEnum.InReview.ToString())
                {
                    taskStatus = TaskStatusEnum.Done.ToString();
                }
            }

            // Optional: fallback if no status change rule matched
            if (taskStatus == null)
            {
                return new CommonReturnDto
                {
                    IsSuccess = false,
                    SuccessMessage = "Invalid status transition."
                };
            }

            var getTask = _appDbContext.TaskItems.FirstOrDefault(t => t.Id == dto.Id && t.MemberId == dto.MemberId);

            if (getTask == null)
            {
                return new CommonReturnDto
                {
                    IsSuccess = false,
                    ErrorMessage = "Task not found."
                };
            }
            // Update fields
            _iMaper.Map(dto, getTask);

            // Correct enum parse
            if (Enum.TryParse<TaskStatusEnum>(taskStatus, out var parsedStatus))
            {
                getTask.Status = parsedStatus;
            }
            else
            {
                return new CommonReturnDto
                {
                    IsSuccess = false,
                    SuccessMessage = "Invalid task status provided."
                };
            }

            try
            {
                await _appDbContext.SaveChangesAsync();

                return new CommonReturnDto
                {
                    IsSuccess = true,
                    SuccessMessage = CommonAlerts.TaskStatusUpdatedSuccessMessage
                };
            }
            catch (Exception ex)
            {
                return new CommonReturnDto
                {
                    IsSuccess = false,
                    SuccessMessage = $"{CommonAlerts.TaskStatusUpdatedErrorMessage} {ex.Message}"
                };
            }

        }
    }

}
