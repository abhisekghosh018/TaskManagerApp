using DevTaskTracker.Application.DTOs.TaskDtos;
using DevTaskTracker.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DevTaskTracker.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITask _itask;

        public TasksController(ITask ITask)
        {
            _itask = ITask;
        }
        // GET: api/<TasksController>
        [Authorize(Roles = "SuperAdmin,Admin,OrgAdmin,User")]
        [HttpGet("gettasks")]
        public async Task<IActionResult> GetTasks(int pageNum)
        {
            var result = await _itask.GetTasksAsync(User, pageNum);
            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);
            return Ok(result.Data);
        }

        // POST api/<TasksController>
        [Authorize(Roles = "SuperAdmin,Admin,OrgAdmin")]
        [HttpPost("createtask")]
        public async Task<IActionResult> CreateTask(CreateTaskItemDto dto)
        {
            var result = await _itask.CreateTaskAsync(dto);
            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);
            return Ok(result);
        }
        [Authorize(Roles = "SuperAdmin,Admin,OrgAdmin,User")]
        [HttpGet("filterTasks")]
        public async Task<IActionResult> FilterTasks(string? priority, string? status, DateTime? dueDate)
        {
            var result = await _itask.FilterTasksByIdStstusprioritydueDateAsync(priority, status, dueDate);

            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);
            return Ok(result.Data);
        }
        [Authorize(Roles = "SuperAdmin,Admin,OrgAdmin,User")]
        [HttpPut("updatetaskstatus")]
        public async Task<IActionResult> UpdateTaskStatus(UpdateStatusDto updateStatusDto)
        {
            if (updateStatusDto == null)
            {
                return BadRequest("Please select a task.");
            }

            var result = await _itask.UpdateTaskStatusAsync(updateStatusDto);

            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result);

        }

    }
}
