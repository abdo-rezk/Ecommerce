using Core.Entities;
using Core.Spacifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastrucure.Data
{
    public class SpacificationEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpacification<T> spac)
        {
            var query = inputQuery;
            if (spac.Criteria != null)
                query = query.Where(spac.Criteria);
            query = spac.Includes.Aggregate(query, (current, include) => current.Include(include));
            return query;
        }
    }
}
