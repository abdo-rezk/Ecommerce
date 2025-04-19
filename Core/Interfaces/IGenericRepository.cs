using Core.Entities;
using Core.Spacifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyCollection<T>> GetAllAsync();

        //Get one entity with spec
        Task<T> GetEntityWithSpec(ISpacification<T> spac);
        //Get all entities with spec
        Task<IReadOnlyList<T>> ListAsync(ISpacification<T> spac);
    }
}
