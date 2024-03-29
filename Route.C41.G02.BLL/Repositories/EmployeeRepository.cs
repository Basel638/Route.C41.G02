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
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _dbContext;


        public EmployeeRepository(ApplicationDbContext dbContext) //Ask CLR For Creating object from "ApllicationDbcontext"
        {
            _dbContext = dbContext;

        }
        public int Add(Employee entity)
        {
            _dbContext.Employees.Add(entity);
            return _dbContext.SaveChanges();
        }

        public int Delete(Employee entity)
        {
            _dbContext.Employees.Remove(entity);
            return _dbContext.SaveChanges();
        }

        public Employee Get(int id)
        {
            ///var Employee = _dbContext.Employees.Local.Where(D => D.Id == id).FirstOrDefault();
            ///
            ///if(Employee==null)
            ///	Employee=_dbContext.Employees.Where(D => D.Id == id).FirstOrDefault();
            ///
            ///return Employee;


            //return _dbContext.Employees.Find(id);

            return _dbContext.Find<Employee>(id);  //EF Core 3.1 Feature

        }

        public IEnumerable<Employee> GetAll()
        {
            return _dbContext.Employees.AsNoTracking().ToList();
        }

        public int Update(Employee entity)
        {
            _dbContext.Employees.Update(entity);
            return (_dbContext.SaveChanges());
        }
    }
}
