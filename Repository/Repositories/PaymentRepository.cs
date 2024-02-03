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
    public class PaymentRepository(DatabaseContext dbcontext) : BaseRepository<Payment>(dbcontext)
    {
        public override Expression<Func<Payment, bool>> GetByIdExpression(Payment item)
        {
            return c => c.PaymentId == item.PaymentId;
        }
    }
}
