using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Spacifications
{
    public interface ISpacification<T>
    {
        //id دى عشان مثلا اعمل  فيلتريشن بشرط معين وليكن  
        Expression<Func<T, bool>> Criteria { get; }

        // دى عشان لو فى انكلود ولا حاجه اعرف ارجعه حاجه زى النفجيشن بروبرتى 
        List<Expression<Func<T, object>>> Includes { get; } 

        Expression<Func<T, object>> OrderBy { get; }
        Expression<Func<T, object>> OrderByDescending { get; }
    }
}
