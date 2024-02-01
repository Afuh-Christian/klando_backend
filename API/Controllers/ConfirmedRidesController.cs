


using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Repository.Repositories;

namespace API.Controllers
{
    public class ConfirmedRidesController : BaseController<ConfirmedRide>
    {
        public ConfirmedRidesController(ConfirmedRideRepository baseRepository) : base(baseRepository)
        {
        }
    }
}