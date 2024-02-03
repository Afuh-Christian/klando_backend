using DataAccess.DataContext;
using Model;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class LocationRepository(DatabaseContext dbcontext) : BaseRepository<Location>(dbcontext)
    {
        public override Expression<Func<Location, bool>> GetByIdExpression(Location item)
        {
            return c => c.Id == item.Id;
        }
    }
}
