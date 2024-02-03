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
    public class OrderRepository(DatabaseContext dbcontext) : BaseRepository<Order>(dbcontext)
    {


        public override Expression<Func<Order, bool>> GetByIdExpression(Order item)
        {
            return c => c.OrderId == item.OrderId;
        }

    }
}
