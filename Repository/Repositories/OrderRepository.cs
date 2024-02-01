using DataAccess.DataContext;
using Model;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class OrderRepository : BaseRepository<Order>
    {
        public OrderRepository(IDatabaseContext dbcontext) : base(dbcontext)
        {
        }
    }
}
