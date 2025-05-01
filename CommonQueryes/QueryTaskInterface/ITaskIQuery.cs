using DevTaskTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonQueryes.QueryMember
{
    public interface ITaskIQuery
    {
        Task<IQueryable<TaskItem>> GetIQueryMembers();
    }
}
