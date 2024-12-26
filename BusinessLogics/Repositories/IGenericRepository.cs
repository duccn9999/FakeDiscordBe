namespace BusinessLogics.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(object id);
        void Insert(T model);
        void Update(T model);
        void Delete(object id);
    }
}
