using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Repository.Repositories;

namespace API.Controllers
{
    public class TripsController : BaseController<Trip>
    {
        public TripsController(TripRepository baseRepository) : base(baseRepository)
        {
        }
    }
    }
