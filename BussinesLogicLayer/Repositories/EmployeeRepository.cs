using BussinesLogicLayer.Interfaces;
using DataAccessLayer.Data;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesLogicLayer.Repositories
{
    public class EmployeeRepository(DataContext context) : IEmployeeRepository
    {
        public void Create(Employee employee)
        {
            context.Employees.Add(employee);
           
        }

        public void Delete(Employee employee)
        {
            context.Employees.Remove(employee);
           
        }

        public Employee? Get(int id)
        {
            return context.Employees.Find(id);
        }

        public IEnumerable<Employee> GetAll()
        {
            return context.Employees.ToList();
        }




        public void Update(Employee employee)
        {
            context.Employees.Update(employee);
            
        }
        public IEnumerable<Employee> GetAll(string Name) => context.Employees.
            Where(e => e.Name.ToLower().Contains( Name.ToLower())).
            Include(e => e.Department).ToList();

        public IEnumerable<Employee> GetAllWithDepartments() => context.Employees.Include(e => e.Department).ToList();
      
           
        
    }
}
