using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Hosting;
using  Route.C41.G02.BLL.Interfaces;
using Route.C41.G02.BLL.Repositories;
using Route.C41.G02.DAL.Models;
using System;

namespace Route.C41.G02.PL.Controllers
{
    // Inheritance : DepartmentController is Controller
    // Association(Composition) : DepartmentController is a DepartmentRepository
    public class DepartmentController : Controller
    {
		private readonly  IUnitOfWork _unitOfWork;

		//private IDepartmentRepository _departmentsRepo; //NULL
		private readonly IWebHostEnvironment _env;

        public DepartmentController( IUnitOfWork unitOfWork, IWebHostEnvironment env) //Ask CLR for Creating an Object from class Imolementing IDepartmentRepository
        {
			/*new DepartmentRepository();*/

			_unitOfWork = unitOfWork;
			_env = env;
        }

        // /Department/Index

        public IActionResult Index()
        {
            var departments = _unitOfWork.Repository<Department>().GetAll();
            return View(departments);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Department department)
        {
            if (ModelState.IsValid) //Server Side Validation
            {
                 _unitOfWork.Repository<Department>().Add(department);
                var count = _unitOfWork.Complete();
                if (count > 0)
                    return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        [HttpGet]

        public IActionResult Details(int? id, string viewname = "Details")
        {
            if (!id.HasValue)
                return BadRequest();

            var department = _unitOfWork.Repository<Department>().Get(id.Value);

            if (department is null)
                return NotFound();

            return View(viewname, department);
        }

        public IActionResult Edit(int? id)
        {
            return Details(id);

            ///		if(!id.HasValue)
            ///		return BadRequest();
            ///
            ///	var department = _departmentsRepo.Get(id.Value);	
            ///	if(department is null)
            ///		return NotFound();
            ///
            ///	return View(department);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, Department department)
        {
            if (id != department.Id)
                return BadRequest();
            if (ModelState.IsValid)
                return View(department);

            try
            {
                _unitOfWork.Repository<Department>().Update(department);
                _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // 1.Log Exception
                // 2.Friendly Message

                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Has Ocurred during Updating The Department");

                return View(department);

            }
        }

        // /Department/Delete/10
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        public IActionResult Delete(Department department)
        {
            try
            {

                _unitOfWork.Repository<Department>().Delete(department);
                _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // 1. Log Exception
                // 2. Friendly Message

                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Has Ocurred during Updating The Department");

                return View(department);
                
            }
        }
    }
}
