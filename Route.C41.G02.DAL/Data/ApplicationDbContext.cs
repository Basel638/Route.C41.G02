using Microsoft.EntityFrameworkCore;
using Route.C41.G02.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Route.C41.G02.DAL.Data
{
	public class ApplicationDbContext:DbContext
	{


        public ApplicationDbContext( DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }

        
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		}
		public DbSet<Department> Departments { get; set; }
    }
}
