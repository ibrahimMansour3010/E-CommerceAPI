using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            return await Table.FindAsync(id);
        }
        public async Task<T> Add(T entity)
        {
            await Table.AddAsync(entity);
            await Context.SaveChangesAsync();
            return entity;
        }
        public async Task<T> Update(T entity)
        {
            Table.Update(entity);
            await Context.SaveChangesAsync();
            return entity;
        }
        public async Task<T> Delete(string id)
        {
            T entity = Get(id).Result;
            Table.Remove(entity);
            await Context.SaveChangesAsync();
            return entity;
        }
    }
}
