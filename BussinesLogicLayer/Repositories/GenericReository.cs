using BussinesLogicLayer.Interfaces;
using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesLogicLayer.Repositories
{
    public class GenericReository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private DataContext Context;
        private DbSet<TEntity> EntitySet;
        public GenericReository(DataContext context)
        {
            Context= context;
            EntitySet = Context.Set<TEntity>();
        }
        public void Create(TEntity TEntity) => EntitySet.Add(TEntity);

        public void Delete(TEntity TEntity) => EntitySet.Remove(TEntity);

        public TEntity? Get(int id) => EntitySet.Find(id);

        public IEnumerable<TEntity> GetAll() => EntitySet.ToList();

        public void Update(TEntity TEntity) => EntitySet.Update(TEntity);
    }
}
