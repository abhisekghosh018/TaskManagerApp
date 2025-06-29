using DevTaskTracker.Domain.Entities;
using DevTaskTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace DevTaskTracker.Infrastructure.Common
{
    public class CommonImplementations<T> : ICommonImplementations<T> where T : class
    {
        private readonly AppDbContext _context;

        public CommonImplementations(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<T> GetIQueryTaskItem()
        {
            return _context.Set<T>();
        }

        public IQueryable<T> GetQueryableIncluding(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            foreach (var include in includes)
            {
                //if (include.Body is UnaryExpression unary && unary.Operand is MemberExpression)
                //{
                //    query = query.Include(Expression.Lambda<Func<T, object>>(unary.Operand, include.Parameters));
                //}
                //else
                //{
                //    query = query.Include(include);
                //}
                query = query.Include(include);
            }

            return query;
        }


        //public async Task<List<T>> Pagination(IQueryable<TaskItem> query, int pageNum)
        //{
        //    var item = await query
        //        .Skip((pageNum - 1) * 10)
        //        .Take(10)
        //        .ToListAsync();
        //    return item;
        //}

        public async Task<List<T>> Pagination(IQueryable<T> query, int pageNum)
        {
            if (pageNum < 1)
                pageNum = 1;
            return await query
            .Skip((pageNum - 1) * 10)
            .Take(10)
            .ToListAsync();
        }
    }

}
