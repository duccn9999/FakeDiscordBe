namespace BusinessLogics.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(object id);
        void Insert(T model);
        void Update(T model);
        void Delete(object id);

        // Asynchronous
        Task<IAsyncEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(object id);
        Task InsertAsync(T model);
    }
}
