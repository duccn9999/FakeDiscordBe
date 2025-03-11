namespace BusinessLogics.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(object id);
        void Insert(T model);
        void InsertRange(List<T> models);
        void Update(T model);
        void Delete(int id);
        void DeleteRange(List<T> models);

        // Asynchronous
        Task InsertRangeAsync(List<T> models);
        Task<IAsyncEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(object id);
        Task InsertAsync(T model);
    }
}
