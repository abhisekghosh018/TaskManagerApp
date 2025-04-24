using DevTaskTracker.Application.DTOs;
using DevTaskTracker.Domain.Entities;
using DevTaskTracker.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DevTaskTracker.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public TasksController(AppDbContext appDbContext) 
        { 
            _appDbContext = appDbContext;
        }


        // GET: api/<TasksController>
        
        [HttpGet("gettasks")]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks()
        {
            return await _appDbContext.TaskItems.Include(t=> t.AssignedToUser).ToListAsync();
        }

        
        // POST api/<TasksController>
       
        [HttpPost("createtask")]
        public async Task<ActionResult<TaskItem>> CreateTask(TaskItemDto dto)
        {
            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                AssignedToUserId = dto.AssignedToUserId
            };

            _appDbContext.TaskItems.Add(task);
            await _appDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTasks), new { id = task.Id }, task);
        }

        
    }
}
