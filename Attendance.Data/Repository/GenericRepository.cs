using System;
using System.Collections.Generic;
using System.Linq;

using System.Linq.Expressions;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Data.Repository
{
    public class GenericRepository : IGenericRepository
    {
        private DataEntity context;
        public GenericRepository()
        {
            context = new DataEntity();
        }

        public void Add<T>(T item) where T : class
        {
            context.Set<T>().Add(item);
            context.SaveChanges();
        }

        public IEnumerable<T> Get<T>() where T : class
        {
            return context.Set<T>().ToList();
        }


        public IQueryable<T> Get<T>(params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            IQueryable<T> query = context.Set<T>();
            foreach( var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public T Get<T>(int? id) where T : class
        {
            return context.Set<T>().Find(id);
        }

        public void Remove<T>(int id) where T : class
        {
            var delete = context.Set<T>().Find(id);
            context.Set<T>().Remove(delete);
            context.SaveChanges();
        }

        public void Update<T>(int id, T item) where T : class
        {
            if(context.Entry<T>(item).State  == EntityState.Detached)
            {
                context.Set<T>().Attach(item);

            }
            context.Entry(item).State = EntityState.Modified;
            context.SaveChanges();
        }

        public IEnumerable<T> Execute<T>(string sql) where T : class
        {
            return context.Database.SqlQuery<T>(sql);
        }
    }
}
