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
    public class ClientRepository : BaseRepository<Client>
    {
        public ClientRepository(IDatabaseContext dbcontext) : base(dbcontext)
        {
        }

        

    }
}
