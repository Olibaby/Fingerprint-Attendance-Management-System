using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Data.Repository
{
    public interface IGenericRepository
    {
        IEnumerable<T> Get<T>() where T : class;
        T Get<T>(int? id) where T : class;
        IQueryable<T> Get<T>(params Expression<Func<T, object>>[] includeProperties) where T : class;
        void Add<T>(T item) where T : class;
        void Update<T>(int id, T item) where T : class;
        void Remove<T>(int id) where T : class;
        IEnumerable<T> Execute<T>(string sql) where T : class;
    }
}
