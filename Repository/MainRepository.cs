using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class MainRepository<T>:IMainRepository<T> where T : BaseModel
    {
        Context Context;
        DbSet<T> Table;
        public MainRepository(Context context)
        {
            Context = context;
            Table = Context.Set<T>();
        }
        public async Task<IEnumerable<T>> Get()
        {
            return  Table;
        }
        public async Task<T> Get(string id)
        {
            return await Table.Where(i=>i.ID == id).AsNoTracking<T>().FirstOrDefaultAsync();
        }
        public async Task<T> Add(T entity)
        {
            await Table.AddAsync(entity);
            await Context.SaveChangesAsync();
            return entity;
        }
        public async Task<IEnumerable<T>> AddRange(IEnumerable<T> entity)
        {
            await Table.AddRangeAsync(entity);
            await Context.SaveChangesAsync();
            return entity;
        }
        public async Task<T> Update(T entity)
        {
            Context.Update(entity);
            await Context.SaveChangesAsync();
            return entity;
        }
        public async Task<IEnumerable<T>> Update(IEnumerable<T> entity)
        {
            Table.UpdateRange(entity);
            await Context.SaveChangesAsync();
            return entity;
        }
        public async Task<T> Delete(string id)
        {
            T entity = await Get(id);
            Table.Remove(entity);
            await Context.SaveChangesAsync();
            return entity;
        }
        public async Task<IEnumerable<T>> Delete(IEnumerable<T> entity)
        {
            Table.RemoveRange(entity);
            await Context.SaveChangesAsync();
            return entity;
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        {
            return Context.Set<T>().Where(expression);
        }
    }
}
