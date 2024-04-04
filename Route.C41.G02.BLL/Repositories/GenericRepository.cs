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
		public void Add(T entity)
		=> _dbContext.Set<T>().Add(entity);


		public void Delete(T entity)
		=> _dbContext.Set<T>().Remove(entity);


		public async Task<T> Get(int id)
		{
			///var T = _dbContext.Set<T>().Local.Where(D => D.Id == id).FirstOrDefault();
			///
			///if(T==null)
			///	T=_dbContext.Set<T>().Where(D => D.Id == id).FirstOrDefault();
			///
			///return T;


			//return _dbContext.Set<T>().Find(id);

			return await _dbContext.FindAsync<T>(id);  //EF Core 3.1 Feature

		}

		public async Task<IEnumerable<T>> GetAll()
		{
			if (typeof(T) == typeof(Employee))
				return (IEnumerable<T>)await _dbContext.Employees.Include(E => E.Department).AsNoTracking().ToListAsync();

			return await _dbContext.Set<T>().AsNoTracking().ToListAsync();
		}

		public void Update(T entity)
		=> _dbContext.Set<T>().Update(entity);
		//_dbContext.Update(entity); //EF Core 3.1
	}
}
