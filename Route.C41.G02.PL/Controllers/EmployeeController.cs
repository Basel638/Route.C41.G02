using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using Route.C41.G02.BLL.Interfaces;
using Route.C41.G02.BLL.Repositories;
using Route.C41.G02.DAL.Models;
using Route.C41.G02.PL.Helpers;
using Route.C41.G02.PL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace Route.C41.G02.PL.Controllers
{
	public class EmployeeController : Controller
	{
		private readonly IMapper _mapper;
		private readonly IWebHostEnvironment _env;
		private readonly IUnitOfWork _unitOfWork;

		//private IEmployeeRepository _employeesRepo; //NULL
		//private readonly IDepartmentRepository _departmentRepository;

		public EmployeeController(IMapper mapper, IWebHostEnvironment env, IUnitOfWork unitOfWork) //Ask CLR for Creating an Object from class Imolementing IEmployeeRepository
		{
			_mapper = mapper;
			_env = env;
			_unitOfWork = unitOfWork;

			/*new EmployeeRepository();*/
			//_employeesRepo = employeesRepo;
			//_departmentRepository = departmentRepository;
		}

		// /Employee/Index

		public async Task<IActionResult> Index(string searchInp)
		{

			var employees = Enumerable.Empty<Employee>();

			var employeeRepo = _unitOfWork.Repository<Employee>() as EmployeeRepository;

			if (string.IsNullOrEmpty(searchInp))
				employees =await employeeRepo.GetAll();
			else
				employees = employeeRepo.SearchByName(searchInp.ToLower());


			var mappedEmps = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);

			return View(mappedEmps);


			///Binding Through View's Dictionary : Transfer Data From Action to View => [One Way] 
			/// 1. ViewData
			///ViewData["Message"] = "Hello ViewData";
			/// 2. ViewBag
			///ViewBag.Message = "Hello ViewBag";


		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(EmployeeViewModel EmployeeVM)
		{
			if (ModelState.IsValid) //Server Side Validation
			{
				EmployeeVM.ImageName =await DocumentSetting.UploadFile(EmployeeVM.Image, "images");

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

				var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(EmployeeVM);
				
				_unitOfWork.Repository<Employee>().Add(mappedEmp);


				// 3. TempData
				///	if (count > 0)
				///		TempData["Message"] = "Department is Created Successfully";
				///
				///	else
				///		TempData["Message"] = "An Error Has Occured, Department Not Created :(";



				// 2. Update Department
				//_unitOfWork.Repository<Department>.Update(department);


				// 3. Delete Project
				//_unitOfWork.Repository<Project>.Remove(project);


				var count =await _unitOfWork.Complete();

				if (count > 0)
				{
					return RedirectToAction(nameof(Index));
				}
			}
			return View(EmployeeVM);
		}

		[HttpGet]

		public async Task<IActionResult> Details(int? id, string viewname = "Details")
		{
			if (!id.HasValue)
				return BadRequest();

			var Employee =await _unitOfWork.Repository<Employee>().Get(id.Value);

			var mappedEmp = _mapper.Map<Employee, EmployeeViewModel>(Employee);

			if (Employee is null)
				return NotFound();

			if(viewname.Equals("Delete",StringComparison.OrdinalIgnoreCase))
			TempData["ImageName"] = Employee.ImageName;

			return View(viewname, mappedEmp);
		}

		public async Task<IActionResult> Edit(int? id)
		{
			//ViewData["Departments"] = _departmentRepository.GetAll();

			return await Details(id);

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
		public async Task<IActionResult> Edit([FromRoute] int id, Employee Employee, EmployeeViewModel employeeVM)
		{
			if (id != Employee.Id)
				return BadRequest();
			if (ModelState.IsValid)
				return View(Employee);

			try
			{
				var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

				_unitOfWork.Repository<Employee>().Update(mappedEmp);
				await _unitOfWork.Complete();
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
		public async Task<IActionResult> Delete(int? id)
		{

			return await Details(id, "Delete");
		}

		[HttpPost]
		public async Task<IActionResult> Delete(Employee Employee, EmployeeViewModel employeeVM)
		{
			try
			{ 


				employeeVM.ImageName = TempData["ImageName"] as string;

				var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
				_unitOfWork.Repository<Employee>().Delete(mappedEmp);
			var count=await	_unitOfWork.Complete();
				if (count > 0)
				{
					DocumentSetting.DeleteFile(employeeVM.ImageName, "images");
				return RedirectToAction(nameof(Index));
				}
				return View(employeeVM);
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
