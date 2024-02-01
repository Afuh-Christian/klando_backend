using Model;
using Repository.Repositories;

namespace API.Controllers
{
    public class DriversController : BaseController<Driver>
    {
        public DriversController(DriverRepository baseRepository) : base(baseRepository)
        {
        }
    }
}