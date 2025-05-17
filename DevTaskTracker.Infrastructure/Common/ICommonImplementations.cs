using DevTaskTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DevTaskTracker.Infrastructure.Common
{
    public interface ICommonImplementations<T> where T : class
    {
        IQueryable<T> GetIQueryTaskItem();
        //IQueryable<T> GetIQueryTaskItemIncludeMember();
        IQueryable<T> GetQueryableIncluding(params Expression<Func<T, object>>[] includes);
        Task<List<T>> Pagination(IQueryable<T> query,int pageNum);
    }
}
