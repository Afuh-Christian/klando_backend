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
    public class TransactionRepository(DatabaseContext dbcontext) : BaseRepository<Transaction>(dbcontext)
    {
        public override Expression<Func<Transaction, bool>> GetByIdExpression(Transaction item)
        {
            return c => c.Id == item.Id;
        }
    }
}
