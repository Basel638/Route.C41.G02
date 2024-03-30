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
    internal class EmolyeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            // Fluent APIs for Employee Domain
            builder.Property(e => e.Name).HasColumnType("varchar").HasMaxLength(50).IsRequired();
            builder.Property(E => E.Address).IsRequired();
            builder.Property(E => E.Salary).HasColumnType("decimal(12,2)");

            builder.Property(E=>E.Gender).HasConversion((Gender)=>Gender.ToString(),(genderAsString)=> (Gender)Enum.Parse(typeof(Gender), genderAsString,true));

            builder.Property(E => E.Name)
                .IsRequired(true)
                .HasMaxLength(50); 
        
        }
    }
}
