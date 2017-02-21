using System.Collections.Generic;
using System.Linq;

namespace TaxDataUpload.DB
{
    public interface ITaxDataUploadDB
    {
        IQueryable<T> Query<T>() where T : class;
        void Add<T>(T entity) where T : class;
        void AddRange<T>(IEnumerable<T> entityList) where T : class;
        void Update<T>(T entity) where T : class;
        void Remove<T>(T entity) where T : class;
        void SaveChanges();
        void Dispose();
    }
}
