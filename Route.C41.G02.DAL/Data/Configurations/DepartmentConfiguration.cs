using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Route.C41.G02.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.C41.G02.DAL.Data.Configurations
{
	internal class DepartmentConfiguration : IEntityTypeConfiguration<Department>
	{
		public void Configure(EntityTypeBuilder<Department> builder)
		{
			//Fluent APIs for "Department" Domain

			builder.Property(D => D.Id).UseIdentityColumn(10, 10);
			builder.Property(D => D.Code).HasColumnType("varchar").HasMaxLength(50).IsRequired();
			builder.Property(D => D.Name).HasColumnType("varchar").HasMaxLength(50).IsRequired();

		}
	}
}
