using DataAccess.DataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected IDatabaseContext _dbcontext ;
        public BaseRepository(IDatabaseContext dbcontext)
        {
            this._dbcontext = dbcontext;
        }

        DbSet<T> list => _dbcontext.Set<T>();

        public virtual Task<T> AddOrUpdate(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual Task Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<T> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await list.ToListAsync();

        }
   
    }
}
