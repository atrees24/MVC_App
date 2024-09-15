using BussinesLogicLayer.Interfaces;
using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesLogicLayer.Repositories
{
    public class UniteOfWork : IUniteOfWork
    {
                private readonly IEmployeeRepository EmployeeRepository;
                private readonly IDepartment DepartmentRepository;
                private readonly DataContext DataContext;

        public UniteOfWork(IEmployeeRepository employeeRepository, IDepartment departmentRepository, DataContext dataContext)
        {
            EmployeeRepository = employeeRepository;
            DepartmentRepository = departmentRepository;
            DataContext = dataContext;
        }

        public IEmployeeRepository Employee => EmployeeRepository;

        public IDepartment Department => DepartmentRepository;

        public int SaveChanges() => DataContext.SaveChanges();


    }
}
