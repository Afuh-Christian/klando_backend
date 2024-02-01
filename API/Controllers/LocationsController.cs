

using Model;
using Repository.Repositories;

namespace API.Controllers
{
    public class LocationsController : BaseController<Location>
    {
        public LocationsController(LocationRepository baseRepository) : base(baseRepository)
        {
        }
    }
}