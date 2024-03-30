using Route.C41.G02.BLL.Interfaces;
using Route.C41.G02.BLL.Repositories;
using Route.C41.G02.DAL.Data;
using Route.C41.G02.DAL.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.C41.G02.BLL
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _dbContext;


		//private Dictionary<string, IGenericRepository<ModelBase>> repositories;

		private Hashtable _repositories;


		public IEmployeeRepository EmployeeRepository { get; set; }
		public IDepartmentRepository DepartmentRepository { get; set; }


		public UnitOfWork(ApplicationDbContext dbContext) //Ask CLR for Creating Object from 'DbContext'
		{

			_dbContext = dbContext;
			EmployeeRepository = new EmployeeRepository(_dbContext);
			DepartmentRepository = new DepartmentRepository(_dbContext);
		}

		public int Complete()
		{
			return _dbContext.SaveChanges();
		}

		public void Dispose()
		{
			_dbContext.Dispose();
		}

		public IGenericRepository<T> Repository<T>() where T : ModelBase
		{
			var key = typeof(T).Name;
			if (!_repositories.ContainsKey(key))
			{


				if (key == nameof(Employee))
				{
					var repository = new EmployeeRepository(_dbContext);
					_repositories.Add(key, repository);
				}
			}
			else
			{
				var repository = new GenericRepository<T>(_dbContext);
					_repositories.Add(key, repository);

			}


			return _repositories[key] as IGenericRepository<T>;
		}
	}
}
