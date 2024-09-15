using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesLogicLayer.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        void Create(TEntity TEntity);
        void Delete(TEntity TEntity);
        TEntity? Get(int id);
        IEnumerable<TEntity> GetAll();
        void Update(TEntity TEntity);
    }
}
