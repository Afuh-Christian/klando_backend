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
    public class TripRepository(DatabaseContext dbcontext) : BaseRepository<Trip>(dbcontext)
    {



        public override Expression<Func<Trip, bool>> GetByIdExpression(Trip item)
        {
            return c => c.TripId == item.TripId;
        }
    }
}
