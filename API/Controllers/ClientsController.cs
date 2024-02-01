using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Repository.Interface;
using Repository.Repositories;

namespace API.Controllers
{

    public class ClientsController : BaseController<Client>
    {
        public ClientsController(ClientRepository baseRepository) : base(baseRepository)
        {
        }

      //  public ClientRepository Repository => Repository as ClientRepository; 
    }
}
