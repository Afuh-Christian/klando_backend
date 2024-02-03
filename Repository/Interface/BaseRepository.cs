using DataAccess.DataContext;
using Microsoft.EntityFrameworkCore;
using Model.DatabaseModels;
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
        try
        {
            var found = await Get(entity);
            if (found != null)
            {
                this.List.Remove(found);
            }
            this.List.Add(entity);
            _ = await this._dbcontext.SaveChangesAsync();
        }catch (Exception ex)
        {
            
        }
        return entity;

    }

 

    public virtual async Task<T> Get(T item)
    {
        return await this.List.FirstOrDefaultAsync(GetByIdExpression(item));
           // c =>  typeof(T).GetField("id", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public).GetValue(c) == typeof(T).GetField("id", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public).GetValue(item) );
    }

    
    public virtual async Task<IEnumerable<T>> GetAll()
    {
        return await this.List.ToListAsync();
    }
    



     public virtual async Task<SuccessObject> Delete(T entity)
    {
        try
        {
            await this.List
                .Where(GetByIdExpression(entity))
                .ExecuteDeleteAsync();
        }
        catch (Exception ex)
        {
            return new SuccessObject(false, ErrorMessage(ex));
        }

        return new SuccessObject();
    }



    public virtual Expression<Func<T, bool>> GetByIdExpression(T item) { return t => false; }

    public string ErrorMessage(Exception ex) {
        return $"message : {ex.Message}";
    }
}
