using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesLogicLayer.Interfaces
{
    public interface IUniteOfWork
    {
        public IEmployeeRepository Employee { get; }
        public IDepartment Department { get; }
        public int SaveChanges();
    }
}
