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
    public class TripRepository : BaseRepository<Trip>
    {
        public TripRepository(IDatabaseContext dbcontext) : base(dbcontext)
        {
        }
    }
}
