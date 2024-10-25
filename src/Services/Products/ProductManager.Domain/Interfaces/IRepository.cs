namespace ProductManager.Domain.Interfaces
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAll(int pageNumber, int pageSize);

        Task<T> Get(int id);

        Task<IEnumerable<T>> GetByName(string namePart);

        Task<T> Add(T item);

        Task Edit(T item);

        Task SoftDelete(int id);
    }
}

