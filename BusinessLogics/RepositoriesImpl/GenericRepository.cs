﻿using AutoMapper;
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
        public void Delete(int id)
        {
            T existing = table.Find(id);
            table.Remove(existing);
        }

        public void DeleteRange(List<T> models)
        {
            table.RemoveRange(models);
        }

        public IEnumerable<T> GetAll()
        {
            return table.AsEnumerable();
        }

        public async Task<IAsyncEnumerable<T>> GetAllAsync()
        {
            var t = table.AsAsyncEnumerable();
            return t;
        }


        public T GetById(object id)
        {
            return table.Find(id);
        }

        public async Task<T> GetByIdAsync(object id)
        {
            var obj = table.FindAsync(id);
            return await obj;
        }

        public void Insert(T obj)
        {
            table.Add(obj);
        }

        public async Task InsertAsync(T model)
        {
            var t = table.AddAsync(model);
            await t;
        }

        public void InsertRange(List<T> models)
        {
            table.AddRange(models);
        }

        public async Task InsertRangeAsync(List<T> models)
        {
            var t = table.AddRangeAsync(models);
            await t;
        }

        public void Update(T obj)
        {
            table.Update(obj);
        }
    }
}
