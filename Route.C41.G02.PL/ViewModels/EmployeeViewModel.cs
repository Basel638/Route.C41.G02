﻿using Route.C41.G02.DAL.Models;
using System.ComponentModel.DataAnnotations;
using System;

namespace Route.C41.G02.PL.ViewModels
{
	public class EmployeeViewModel
	{

        public int Id { get; set; }	


        [Required(ErrorMessage = "Name is Required!")]
		[MaxLength(50, ErrorMessage = "Max Length of Name is 50 Chars")]
		[MinLength(5, ErrorMessage = "Min Length of Name is 5 Chars")]
		public string Name { get; set; }

		[Range(22, 30)]
		public int? Age { get; set; }

		[RegularExpression(@"^[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$", ErrorMessage = "Address must be like 123-Street-City-Country")]
		public string Address { get; set; }

		[DataType(DataType.Currency)]
		public decimal Salary { get; set; }

		[Display(Name = "Is Active")]
		public bool IsActive { get; set; }
		[EmailAddress]
		public string Email { get; set; }

		[Display(Name = "Phone Number")]
		[Phone]
		public string PhoneNumber { get; set; }

		[Display(Name = "Hiring Date")]
		public DateTime HiringDate { get; set; }
		public Gender Gender { get; set; }

		public int DepartmentId { get; set; } //Foreign Key Column

		//[InverseProperty(nameof(Models.Department.Employees))]

		public Department Department { get; set; }


	}
}
