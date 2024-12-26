using AutoMapper;
using BusinessLogics.Repositories;
using DataAccesses.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogics.RepositoriesImpl
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly FakeDiscordContext _context;
        protected readonly IMapper _mapper;
        protected DbSet<T> table = null;
        public GenericRepository(FakeDiscordContext context)
        {
            _context = context;
            table = _context.Set<T>();
        }
        public GenericRepository(IMapper mapper)
        {
            _mapper = mapper;
        }
        public void Delete(object id)
        {
            T existing = table.Find(id);
            table.Remove(existing);
        }

        public IEnumerable<T> GetAll()
        {
            return table.AsEnumerable();
        }

        public T GetById(object id)
        {
            return table.Find(id);
        }

        public void Insert(T obj)
        {
            table.Add(obj);
        }

        public void Update(T obj)
        {
            table.Update(obj);
        }
    }
}
