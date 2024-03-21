using Microsoft.AspNetCore.Mvc;
using Route.C41.G02.BLL.Interfaces;
using Route.C41.G02.BLL.Repositories;

namespace Route.C41.G02.PL.Controllers
{
	// Inheritance : DepartmentController is Controller
    // Association(Composition) : DepartmentController is a DepartmentRepository
	public class DepartmentController : Controller
	{

		private IDepartmentRepository _departmentsRepo; //NULL


        public DepartmentController(IDepartmentRepository departmentsRepo) //Ask CLR for Creating an Object from class Imolementing IDepartmentRepository
        {
			 /*new DepartmentRepository();*/

			_departmentsRepo = departmentsRepo;

		}

        // /Department/Index

        public IActionResult Index()
		{
			var departments = _departmentsRepo
			return View();
		}
	}
}
