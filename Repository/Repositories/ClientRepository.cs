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
    public class ClientRepository(DatabaseContext dbcontext) : BaseRepository<Client>(dbcontext)
    {
        public override Expression<Func<Client, bool>> GetByIdExpression(Client item) { 
        
            return  c => c.ClientId == item.ClientId;
        }
    }
}
