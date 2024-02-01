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

        string id = "userId";

        [HttpGet]
        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await baseRepository.GetAll();
        }

        [HttpPost]
        public virtual async Task<T> Get([FromBody] T entity)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public virtual async Task<T> AddOrUpdate([FromBody] T entity)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public virtual async Task<T> Delete([FromBody] T entity)
        {
            throw new NotImplementedException();
        }
        //  public async Task<IEnumerable<T>> GetAll()
        //  {
        //      return await baseRepository.GetAll();
        //  }
    }
}
