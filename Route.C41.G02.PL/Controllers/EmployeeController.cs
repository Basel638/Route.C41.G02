using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using Route.C41.G02.BLL.Interfaces;
using Route.C41.G02.DAL.Models;
using Route.C41.G02.PL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace Route.C41.G02.PL.Controllers
{
	public class EmployeeController : Controller
	{
		private readonly IMapper _mapper;
		private IEmployeeRepository _employeesRepo; //NULL
		private readonly IWebHostEnvironment _env;
		//private readonly IDepartmentRepository _departmentRepository;

		public EmployeeController(IMapper mapper,IEmployeeRepository employeesRepo, IWebHostEnvironment env/*, IDepartmentRepository departmentRepository*/) //Ask CLR for Creating an Object from class Imolementing IEmployeeRepository
		{
			_mapper = mapper;
			/*new EmployeeRepository();*/

			_employeesRepo = employeesRepo;
			_env = env;
			//_departmentRepository = departmentRepository;
		}

		// /Employee/Index

		public IActionResult Index(string searchInp)
		{

			var employees = Enumerable.Empty<Employee>();

			if(string.IsNullOrEmpty(searchInp))
				 employees = _employeesRepo.GetAll();
			else
				 employees = _employeesRepo.SearchByName(searchInp.ToLower());


			var mappedEmps = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);

			return View(mappedEmps);


			//Binding Through View's Dictionary : Transfer Data From Action to View => [One Way] 

			// 1. ViewData
			//ViewData["Message"] = "Hello ViewData";


			// 2. ViewBag
			//ViewBag.Message = "Hello ViewBag";


		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(EmployeeViewModel EmployeeVM)
		{
			if (ModelState.IsValid) //Server Side Validation
			{
				// Manual Mapping
				///var mappedEmp = new Employee()
				///{
				///	Name = EmployeeVM.Name,
				///	Address = EmployeeVM.Address,
				///	Age = EmployeeVM.Age,
				///	Email = EmployeeVM.Email,
				///	HiringDate = EmployeeVM.HiringDate,
				///	Salary = EmployeeVM.Salary,
				///	PhoneNumber = EmployeeVM.PhoneNumber,
				///	IsActive = EmployeeVM.IsActive,
				///};

				//Employee mappedEmp = (Employee)EmployeeVM;
			
				var mappedEmp= _mapper.Map<EmployeeViewModel,Employee>(EmployeeVM);	
				var count = _employeesRepo.Add(mappedEmp);


				// 3. TempData
				if (count > 0)
					TempData["Message"] = "Department is Created Successfully";

				else
					TempData["Message"] = "An Error Has Occured, Department Not Created :(";

				return RedirectToAction(nameof(Index));
			}
			return View(EmployeeVM);
		}

		[HttpGet]

		public IActionResult Details(int? id, string viewname = "Details")
		{
			if (!id.HasValue)
				return BadRequest();

			var Employee = _employeesRepo.Get(id.Value);

			var mappedEmp = _mapper.Map<Employee, EmployeeViewModel>(Employee);

			if (Employee is null)
				return NotFound();

			return View(viewname, mappedEmp);
		}

		public IActionResult Edit(int? id)
		{
			//ViewData["Departments"] = _departmentRepository.GetAll();

			return Details(id);

			///		if(!id.HasValue)
			///		return BadRequest();
			///
			///	var Employee = _EmployeesRepo.Get(id.Value);	
			///	if(Employee is null)
			///		return NotFound();
			///
			///	return View(Employee);


		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit([FromRoute] int id, Employee Employee, EmployeeViewModel employeeVM)
		{
			if (id != Employee.Id)
				return BadRequest();
			if (ModelState.IsValid)
				return View(Employee);

			try
			{
				var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

				_employeesRepo.Update(mappedEmp);
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				// 1.Log Exception
				// 2.Friendly Message

				if (_env.IsDevelopment())
					ModelState.AddModelError(string.Empty, ex.Message);
				else
					ModelState.AddModelError(string.Empty, "An Error Has Ocurred during Updating The Employee");

				return View(Employee);

			}
		}

		// /Employee/Delete/10
		[HttpGet]
		public IActionResult Delete(int? id)
		{
			return Details(id, "Delete");
		}

		[HttpPost]
		public IActionResult Delete(Employee Employee, EmployeeViewModel employeeVM)
		{
			try
			{
				var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
				_employeesRepo.Delete(mappedEmp);
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				// 1. Log Exception
				// 2. Friendly Message

				if (_env.IsDevelopment())
					ModelState.AddModelError(string.Empty, ex.Message);
				else
					ModelState.AddModelError(string.Empty, "An Error Has Ocurred during Updating The Employee");

				return View(Employee);

			}
		}
	}
}
