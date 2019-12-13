using OrderCrateAPI.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using OrderCrateAPI.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OrderCrateAPI.Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected OrdercratedbContext RepositoryContext { get; set; }

        public RepositoryBase(OrdercratedbContext repositoryContext)
        {
            this.RepositoryContext = repositoryContext;
        }

        public async Task<IEnumerable<T>> FindAllAsync()
        {
            try
            {
                return await this.RepositoryContext.Set<T>().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<T>> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return await this.RepositoryContext.Set<T>().Where(expression).ToListAsync();
        }

        public void Create(T entity)
        {
            this.RepositoryContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            this.RepositoryContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            this.RepositoryContext.Set<T>().Remove(entity);
        }

        public async Task Save()
        {
           await this.RepositoryContext.SaveChangesAsync();
        }
    }
}
