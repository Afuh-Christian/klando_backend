namespace API.Interfaces
{
    public interface IBaseController<T> where T : class
    {
        Task<T> Get(string id);
        Task Delete(string id);
        Task<T> AddOrUPdate(T entity);
        Task<IEnumerable<T>> GetAll();
    }
}
