

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Repository.Repositories;

namespace API.Controllers
{
    public class OrdersController : BaseController<Order>
    {
        public OrdersController(OrderRepository baseRepository) : base(baseRepository)
        {
        }
    }
}