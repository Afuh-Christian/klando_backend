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
    public class DriverRepository(DatabaseContext dbcontext) : BaseRepository<Driver>(dbcontext)
    {
        public override Expression<Func<Driver, bool>> GetByIdExpression(Driver item)
        {
            return c => c.DriverId == item.DriverId;
        }
    }
}
