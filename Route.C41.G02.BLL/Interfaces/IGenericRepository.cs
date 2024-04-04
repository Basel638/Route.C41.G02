using Route.C41.G02.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.C41.G02.BLL.Interfaces
{
    public interface IGenericRepository<T>where T: ModelBase
    {
       Task<IEnumerable<T>> GetAll();

        Task<T> Get(int id);

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}
