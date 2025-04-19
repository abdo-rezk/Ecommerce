using Core.Entities;
using Core.Interfaces;
using Core.Spacifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastrucure.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _context;
        public GenericRepository(StoreContext context)
        {
            _context = context;
        }
        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
        public async Task<IReadOnlyCollection<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }
        
        public async Task<T> GetEntityWithSpec(ISpacification<T> spac)
        {
            return await ApplySpecification(spac).FirstOrDefaultAsync();
        }
        public async Task<IReadOnlyList<T>> ListAsync(ISpacification<T> spac)
        {
            return await ApplySpecification(spac).ToListAsync();
        }

        // this method is used to apply the specification to the query
        private IQueryable<T> ApplySpecification(ISpacification<T> spac)
        {
            return SpacificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spac);
        }
    }
}
