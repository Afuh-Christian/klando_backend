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
    public class ImageRepository(DatabaseContext dbcontext) : BaseRepository<Image>(dbcontext)
    {
        public override Expression<Func<Image, bool>> GetByIdExpression(Image item)
        {
            return c => c.Id == item.Id;
        }
    }
}
