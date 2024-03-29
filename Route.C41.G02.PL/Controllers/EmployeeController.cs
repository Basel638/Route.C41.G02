using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Route.C41.G02.BLL.Interfaces;
using Route.C41.G02.DAL.Models;
using System;

namespace Route.C41.G02.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private IEmployeeRepository _employeesRepo; //NULL
        private readonly IWebHostEnvironment _env;

        public EmployeeController(IEmployeeRepository employeesRepo, IWebHostEnvironment env) //Ask CLR for Creating an Object from class Imolementing IEmployeeRepository
        {
            /*new EmployeeRepository();*/

            _employeesRepo = employeesRepo;
            _env = env;
        }

        // /Employee/Index

        public IActionResult Index()
        {
            //Binding Through View's Dictionary : Transfer Data From Action to View => [One Way] 

            // 1. ViewData
            ViewData["Message"] = "Hello ViewData";


            // 2. ViewBag
            ViewBag.Message = "Hello ViewBag";


            var Employees = _employeesRepo.GetAll();
            return View(Employees);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee Employee)
        {
            if (ModelState.IsValid) //Server Side Validation
            {
                var count = _employeesRepo.Add(Employee);


                // 3. TempData
                if (count > 0)
                    TempData["Message"] = "Department is Created Successfully";

                else
                TempData["Message"] = "An Error Has Occured, Department Not Created :(";
           
                return RedirectToAction(nameof(Index));
            }
            return View(Employee);
        }

        [HttpGet]

        public IActionResult Details(int? id, string viewname = "Details")
        {
            if (!id.HasValue)
                return BadRequest();

            var Employee = _employeesRepo.Get(id.Value);

            if (Employee is null)
                return NotFound();

            return View(viewname, Employee);
        }

        public IActionResult Edit(int? id)
        {
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
        public IActionResult Edit([FromRoute] int id, Employee Employee)
        {
            if (id != Employee.Id)
                return BadRequest();
            if (ModelState.IsValid)
                return View(Employee);

            try
            {
                _employeesRepo.Update(Employee);
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
        public IActionResult Delete(Employee Employee)
        {
            try
            {

                _employeesRepo.Delete(Employee);
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
