using CommonQueryes.QueryMember;
using DevTaskTracker.Domain.Entities;
using DevTaskTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonQueryes.QueryClass
{
    public class TaskIQueryService : ITaskIQuery
    {
        private readonly AppDbContext _appDbContext;
        public TaskIQueryService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        Task<IQueryable<TaskItem>> ITaskIQuery.GetIQueryMembers()
        {
            var query = _appDbContext.TaskItems
                .Include(t => t.Member)
                .AsQueryable();

            return Task.FromResult(query);
        }
    }   
   
}
