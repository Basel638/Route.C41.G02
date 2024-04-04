using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Hosting;
using  Route.C41.G02.BLL.Interfaces;
using Route.C41.G02.BLL.Repositories;
using Route.C41.G02.DAL.Models;
using System;
using System.Threading.Tasks;

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

        public async Task<IActionResult> Index()
        {
            var departments =await _unitOfWork.Repository<Department>().GetAll();
            return View(departments);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Department department)
        {
            if (ModelState.IsValid) //Server Side Validation
            {
                 _unitOfWork.Repository<Department>().Add(department);
                var count =await _unitOfWork.Complete();
                if (count > 0)
                    return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        [HttpGet]

        public async Task<IActionResult> Details(int? id, string viewname = "Details")
        {
            if (!id.HasValue)
                return BadRequest();

            var department =await _unitOfWork.Repository<Department>().Get(id.Value);

            if (department is null)
                return NotFound();

            return View(viewname, department);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id);

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
        public async Task<IActionResult> Edit([FromRoute] int id, Department department)
        {
            if (id != department.Id)
                return BadRequest();
            if (ModelState.IsValid)
                return View(department);

            try
            {
                _unitOfWork.Repository<Department>().Update(department);
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
                    ModelState.AddModelError(string.Empty, "An Error Has Ocurred during Updating The Department");

                return View(department);

            }
        }

        // /Department/Delete/10
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Department department)
        {
            try
            {

                _unitOfWork.Repository<Department>().Delete(department);
               await _unitOfWork.Complete();
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
