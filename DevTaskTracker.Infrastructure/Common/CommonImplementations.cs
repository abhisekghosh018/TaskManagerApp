using DevTaskTracker.Domain.Entities;
using DevTaskTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DevTaskTracker.Infrastructure.Common
{
    public class CommonImplementations : ICommonImplementations
    {
        private readonly AppDbContext _context;

        public CommonImplementations(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<TaskItem> GetIQueryTaskItem()
        {
            return _context.TaskItems;
        }
    }

}
