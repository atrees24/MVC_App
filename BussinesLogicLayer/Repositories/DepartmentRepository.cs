using BussinesLogicLayer.Interfaces;
using DataAccessLayer.Data;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesLogicLayer.Repositories
{
    public class DepartmentRepository : IDepartment
    {
        private readonly DataContext _Datacontext;

        public DepartmentRepository(DataContext context)
        {
            _Datacontext = context;
        }

        public Department? Get(int id)
        {
           return _Datacontext.Departments.Find( id);
        }

        public IEnumerable<Department> GetAll()
        {
            return _Datacontext.Departments.ToList();
        }

        public int Create(Department department)
        {
            _Datacontext.Departments.Add( department );
            return _Datacontext.SaveChanges();
        }

        public int Update(Department department)
        {
            _Datacontext.Departments.Update( department );
            return _Datacontext.SaveChanges();
        }

        public int Delete(Department department)
        {
            _Datacontext.Departments.Remove(department );
            return _Datacontext.SaveChanges();
        }
    }
}
