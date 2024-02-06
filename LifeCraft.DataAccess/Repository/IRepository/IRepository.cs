using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LifeCraft.DataAccess.Repository.IRepository
{
    public  interface IRepository<T>  where T : class
    {
        //T (Generic) - Category
        IEnumerable<T> GetAll(string? includeProperties = null);
        //Get a single category obj, this is for the method firstordefault with the link operation : firstOrDefault(u=>u.id == id)
        T Get(Expression<Func<T, bool>> expression, string? includeProperties = null);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
        //Update obj is not written in this interface 

            
    }
}
