using Core.Entities;
using Core.Entities.OrederAggregat;
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
        Task<IReadOnlyList<T>> GetAllAsync();


        // بعمل ال specification  عشان لما بيبقى فيه  include  in generic repository  مكنش بيفهم يدخل جوا ازاى 

        //Get one entity with spec
        Task<T> GetEntityWithSpec(ISpacification<T> spac);
        //
        //Get all entities with spec
        Task<IReadOnlyList<T>> ListAsync(ISpacification<T> spac);

        Task<int> CountAsync(ISpacification<T> spac);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
