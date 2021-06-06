using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using MedicalAppointment_API.DataLayer.Contexts;


namespace MedicalAppointment_API.DataLayer.Generics
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private MedicalAppointmentContext medicalAppointmentContext;

        private DbSet<T> dbSet;

        private bool disposed = false;

        public GenericRepository(MedicalAppointmentContext medicalAppointmentContext)
        {
            this.medicalAppointmentContext = medicalAppointmentContext;
            dbSet = this.medicalAppointmentContext.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return dbSet;
        }

        public T FindBy(int id)
        {
            return dbSet.Find(id);
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return dbSet.Where(predicate);
        }

        public IQueryable<TType> FindBy<TType>(Expression<Func<T, bool>> predicate, Expression<Func<T, TType>> select) where TType : class
        {
            return dbSet.Where(predicate).Select(select);
        }

        public void Insert(T entity)
        {
            dbSet.Add(entity);
        }

        public void Insert(IEnumerable<T> entities)
        {
            dbSet.AddRange(entities);
        }
        public void Edit(T entity)
        {
            medicalAppointmentContext.Entry<T>(entity).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            dbSet.Remove(this.FindBy(id));
        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public void Delete(Expression<Func<T, bool>> predicate)
        {
            dbSet.RemoveRange(this.FindBy(predicate).ToList());
        }

        public void Delete(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }

        public int Save()
        {
           return medicalAppointmentContext.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.medicalAppointmentContext.Dispose();
                }
            }

            this.disposed = true;
        }
    }
}
