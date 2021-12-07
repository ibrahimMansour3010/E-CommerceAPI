using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IMainRepository<T>
    {
        Task<IEnumerable<T>> Get();
        Task<T> Get(string id);
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(string id);
    }
}
