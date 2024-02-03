using DataAccess.DataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
  
    public BaseRepository(DatabaseContext dbcontext)
    {
        this._dbcontext = dbcontext;
    }

    public DatabaseContext _dbcontext;

    DbSet<T> List => this._dbcontext.Set<T>();


    public virtual async Task<T> AddOrUpdate(T entity)
    {
        var found = await Get(entity);
        if (found != null)
        {
            this.List.Remove(found);
        }
        this.List.Add(entity);
        _ = await this._dbcontext.SaveChangesAsync();
        return entity;

    }

    public virtual Task Delete(T entity)
    {
        throw new NotImplementedException();
    }

    public virtual async Task<T> Get(T item)
    {
        return await this.List.FirstOrDefaultAsync(GetByIdExpression(item));
    }

    public virtual async Task<IEnumerable<T>> GetAll()
    {
        return await this.List.ToListAsync();

    }

    public virtual Expression<Func<T, bool>> GetByIdExpression(T item) { return t => false; }

}
