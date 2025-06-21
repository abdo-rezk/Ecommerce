using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Spacifications
{
    public class BaseSpacification<T> : ISpacification<T>
    {
        public BaseSpacification()
        {
        }
        public BaseSpacification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }
       
        public Expression<Func<T, bool>> Criteria { get; }
        
        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

        public Expression<Func<T, object>> OrderBy { get; private set; }


        // private set;  عشان هيحصل سيت بس جوا الكلاس فقط
        public Expression<Func<T, object>> OrderByDescending { get; private set; }

        public int Take{ get; private set; }

        public int Skip { get; private set; }

        public bool IsPagingEnabled { get; private set; }

        protected void AddInclude(Expression<Func<T,object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }
        protected void ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }
        protected void ApplyOrderByDescending(Expression<Func<T, object>> orderByExpression)
        {
            OrderByDescending = orderByExpression;
        }
        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }
    }
}
