using API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Interface;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BaseController<T> : ControllerBase, IBaseController<T> where T : class
    {
        protected IBaseRepository<T> baseRepository; 
        public BaseController(IBaseRepository<T> baseRepository)
        {
            this.baseRepository = baseRepository;
        }

        [HttpPost]
        public Task<T> AddOrUPdate(T entity)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public Task<T> Get(string id)
        {
            Dictionary<string, string> g = new Dictionary<string, string>();

             g["hello"] = (string)"name";

            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<IEnumerable<T>> GetAll()
        {
            return await baseRepository.GetAll();
        }
    }
}
