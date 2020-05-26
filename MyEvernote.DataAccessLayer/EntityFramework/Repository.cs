using MyEvernote.Common;
using MyEvernote.Core.DataAccess;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace MyEvernote.DataAccessLayer.EntityFramework
{
    public class Repository<T> : RepositoryBase, IDataAccess<T> where T : class
    {
        private DbSet<T> _objectSet;

        public Repository()
        {
            _objectSet = context.Set<T>();
        }

        public List<T> List()
        {
            return _objectSet.ToList();
        }

        public IQueryable<T> List(Expression<Func<T, bool>> value)
        {
            return _objectSet.Where(value);
        }

        public int Insert(T obj)
        {
            _objectSet.Add(obj);

            if (obj is MyEntityBase)
            {
                MyEntityBase mt = obj as MyEntityBase;
                DateTime date = DateTime.Now;

                mt.CreatedDate = date;
                mt.UpdatedDate = date;
                mt.CreatedUserName = App.Common.GetCurrentUserName();
            }

            return Save();
        }

        public int Update(T obj)
        {
            if (obj is MyEntityBase)
            {
                MyEntityBase mt = obj as MyEntityBase;

                mt.UpdatedDate = DateTime.Now;
                mt.CreatedUserName = App.Common.GetCurrentUserName();
            }

            return Save();
        }

        public int Delete(T obj)
        {
            _objectSet.Remove(obj);
            return Save();
        }

        public int Save()
        {
            return context.SaveChanges();
        }

        public T Find(Expression<Func<T, bool>> value)
        {
            return _objectSet.FirstOrDefault(value);
        }
    }
}