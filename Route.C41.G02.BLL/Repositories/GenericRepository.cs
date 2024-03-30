using Microsoft.EntityFrameworkCore;
using Route.C41.G02.BLL.Interfaces;
using Route.C41.G02.DAL.Data;
using Route.C41.G02.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.C41.G02.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : ModelBase

    {
        private protected readonly ApplicationDbContext _dbContext;


        public GenericRepository(ApplicationDbContext dbContext) //Ask CLR For Creating object from "ApllicationDbcontext"
        {
            _dbContext = dbContext;

        }
        public int Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            return _dbContext.SaveChanges();
        }

        public int Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            return _dbContext.SaveChanges();
        }

        public T Get(int id)
        {
            ///var T = _dbContext.Set<T>().Local.Where(D => D.Id == id).FirstOrDefault();
            ///
            ///if(T==null)
            ///	T=_dbContext.Set<T>().Where(D => D.Id == id).FirstOrDefault();
            ///
            ///return T;


            //return _dbContext.Set<T>().Find(id);

            return _dbContext.Find<T>(id);  //EF Core 3.1 Feature

        }

        public IEnumerable<T> GetAll()
        {
            if(typeof(T)== typeof(Employee))
				return (IEnumerable<T>)_dbContext.Employees.Include(E=>E.Department).AsNoTracking().ToList();

			return _dbContext.Set<T>().AsNoTracking().ToList();
        }

        public int Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            //_dbContext.Update(entity); //EF Core 3.1

            return (_dbContext.SaveChanges());
        }
    }
}