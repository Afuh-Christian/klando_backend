using DataAccess.DataContext;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
    public class ConfirmedRideRepository(DatabaseContext dbcontext) : BaseRepository<ConfirmedRide>(dbcontext)
    {
        public override Expression<Func<ConfirmedRide, bool>> GetByIdExpression(ConfirmedRide item)
        {
            return c => c.ConfirmedRideId == item.ConfirmedRideId;
        }
    }
}
