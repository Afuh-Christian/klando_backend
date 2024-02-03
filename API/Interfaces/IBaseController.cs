using Microsoft.AspNetCore.Mvc;
using Model.DatabaseModels;

namespace API.Interfaces
{
    public interface IBaseController<T> where T : class
    {

        Task<IEnumerable<T>> GetAll();
        Task<T> Get([FromBody] T entity);
        Task<T> AddOrUpdate([FromBody] T entity);
        Task<SuccessObject> Delete([FromBody] T entity);



    }
}
