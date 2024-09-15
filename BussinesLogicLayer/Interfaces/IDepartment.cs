using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesLogicLayer.Interfaces
{
    public interface IDepartment
    {
        int Create(Department department);
        int Delete(Department department);
        Department? Get(int id);
        IEnumerable<Department> GetAll();
        int Update(Department department);

    }
}
