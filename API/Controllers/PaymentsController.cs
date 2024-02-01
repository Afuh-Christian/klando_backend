

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Repository.Repositories;

namespace API.Controllers
{
    public class PaymentsController : BaseController<Payment>
    {
        public PaymentsController(PaymentRepository baseRepository) : base(baseRepository)
        {
        }
    }
}