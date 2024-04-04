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
	public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
	{


		public EmployeeRepository(ApplicationDbContext dbContext)// Ask CLR for Creating object from "ApplicationDbContext"
		 : base(dbContext)
		{

		}
		public IQueryable<Employee> GetEmployeeByAddress(string address)
		{
			return _dbContext.Employees.Where(E => E.Address.ToLower() == address.ToLower());
		}

		public IQueryable<Employee> SearchByName(string Name)
 		{
			return _dbContext.Employees.Where(E => E.Name.ToLower().Contains(Name));
		}

		
	}
}
