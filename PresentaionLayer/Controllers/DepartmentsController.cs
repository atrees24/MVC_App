using BussinesLogicLayer.Interfaces;
using BussinesLogicLayer.Repositories;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace PresentaionLayer.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly IDepartment Department;

        public DepartmentsController(IDepartment departmentRepository)
        {
            Department = departmentRepository;
        }

        public IActionResult Index()
        {
            var departments = Department.GetAll();
            return View(departments);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Department department)
        {
            if (!ModelState.IsValid) return View(department);
            Department.Create(department);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Details(int? id) => DepartmentControllerHandler(id, nameof(Details));
        //{
            //if (!id.HasValue) return BadRequest();
            //var department = Department.Get(id.Value);
            //if (department is null) return NotFound();
            //return View(department);
        //}


        public IActionResult Edit(int? id) => DepartmentControllerHandler(id, nameof(Edit));
        //{
        //    if (!id.HasValue) return BadRequest();
        //    var department = Department.Get(id.Value);
        //    if (department is null) return NotFound();
        //    return View(department);
        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id ,Department department)
        {
            if (!ModelState.IsValid) return View(department);
            Department.Update(department);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id) => DepartmentControllerHandler(id,nameof(Delete));
        //{
        //    var department = Department.Get(id.Value);
        //    if (department is null) return NotFound();
        //    return View(department);
        //}



        [HttpPost]
        public IActionResult Delete(int id)
        {
            var department = Department.Get(id);
            if (department is null) return NotFound();
            Department.Delete(department);
            return RedirectToAction(nameof(Index));
        }

        private IActionResult DepartmentControllerHandler(int? id, string ViewName)
        {
            if (!id.HasValue) return BadRequest();
            var department = Department.Get(id.Value);
            if (department is null) return NotFound();
            return View( ViewName, department);
        }
    }
}
