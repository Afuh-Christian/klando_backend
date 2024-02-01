using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> Get(T entity);
        Task Delete(T entity);
        Task<IEnumerable<T>> GetAll();
        Task<T> AddOrUpdate(T entity);


       // Task<IEnumerable<T>> GetList(); 
    }
}
