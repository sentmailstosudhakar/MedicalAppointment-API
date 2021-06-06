using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MedicalAppointment_API.DataLayer.Generics
{
    public interface IGenericRepository<T> where T : class
    {
        public IQueryable<T> GetAll();
        public T FindBy(int id);
        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        public IQueryable<TType> FindBy<TType>(Expression<Func<T, bool>> predicate, Expression<Func<T, TType>> select) where TType : class;
        public void Insert(T entity);
        public void Insert(IEnumerable<T> entities);
        public void Edit(T entity);
        public void Delete(int id);
        public void Delete(T entity);
        public void Delete(Expression<Func<T, bool>> predicate);
        public void Delete(IEnumerable<T> entities);
        public int Save();
    }
}
