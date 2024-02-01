using DataAccess.DataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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



        public virtual Expression<Func<T, bool>> GetByIdExpression(T item) { return t => false; }


        public virtual Task<T> AddOrUpdate(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual Task Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<T> Get(T entity)
        {
            return await this.list.FirstOrDefaultAsync(GetByIdExpression(entity));
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await this.list.ToListAsync();

        }
   
    }
}
