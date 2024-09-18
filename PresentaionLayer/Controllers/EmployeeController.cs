using AutoMapper;
using BussinesLogicLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using PresentaionLayer.Utilites;
using PresentaionLayer.ViewModels;

namespace PresentaionLayer.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IUniteOfWork _uniteOfWork;
        private readonly IMapper Mapper;
        public EmployeeController(IUniteOfWork uniteOfWork, IMapper mapper)
        {
             _uniteOfWork = uniteOfWork;
             Mapper = mapper;
        }

        public IActionResult Index(string? SearchValue)
        {
            var Employees = Enumerable.Empty<Employee>();
            if (string.IsNullOrWhiteSpace(SearchValue))
                 Employees = _uniteOfWork.Employee.GetAllWithDepartments();

            else Employees = _uniteOfWork.Employee.GetAll(SearchValue);

            var employeeViewModel = Mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(Employees);
           
            return View(employeeViewModel);
        }

        public IActionResult Create()
        {
            var departments = _uniteOfWork.Department.GetAll();
            SelectList listItems = new SelectList(departments,"Id","Name");
            ViewBag.Departments=listItems;
            return View();
        }


        [HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(EmployeeViewModel employee)
        {
            if (!ModelState.IsValid) return View(employee);

            if(employee.Image is not null)
            employee.ImageName = DocumentSetting.UploadFile(employee.Image,"Image");


            var Emp = Mapper.Map<EmployeeViewModel, Employee>(employee);


            _uniteOfWork.Employee.Create(Emp);
            _uniteOfWork.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Details(int? id) => EmployeeControllerHandler(id, nameof(Details));
        


        public IActionResult Edit(int? id) => EmployeeControllerHandler(id, nameof(Edit));
       


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, EmployeeViewModel employee)
        {
            if (!ModelState.IsValid) return View(employee);

            if (employee.Image is not null)
                employee.ImageName = DocumentSetting.UploadFile(employee.Image, "Image");

            var Emp = Mapper.Map<EmployeeViewModel, Employee>(employee);
            _uniteOfWork.Employee.Update(Emp);
            _uniteOfWork.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id) => EmployeeControllerHandler(id, nameof(Delete));
       



        [HttpPost]
        [ValidateAntiForgeryToken]
		public IActionResult Delete(int id)
        {
            var employee = _uniteOfWork.Employee.Get(id);
            if (employee is null) return NotFound();
            _uniteOfWork.Employee.Delete(employee);
            _uniteOfWork.SaveChanges();
            if (_uniteOfWork.SaveChanges()>0 && employee.ImageName is not null)
            {
                DocumentSetting.DeleteFile("Image",employee.ImageName);
            }
            return RedirectToAction(nameof(Index));
        }

        private IActionResult EmployeeControllerHandler(int? id, string ViewName)
        {
            if (!id.HasValue) return BadRequest();
            var employee = _uniteOfWork.Employee.Get(id.Value);
            if (employee is null) return NotFound();

            if (ViewName == nameof(Edit))
            {
                var departments = _uniteOfWork.Department.GetAll();
                SelectList listItems = new SelectList(departments, "Id", "Name");
                ViewBag.Departments = listItems;
            }

            var EmployeeVM = Mapper.Map<EmployeeViewModel>(employee);

            return View(ViewName, EmployeeVM);
        }

    }
}

