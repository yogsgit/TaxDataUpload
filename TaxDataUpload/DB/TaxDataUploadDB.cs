using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TaxDataUpload.Models;

namespace TaxDataUpload.DB
{
    public class TaxDataUploadDB : DbContext, ITaxDataUploadDB
    {
        public DbSet<TransactionData> TransactionData { get; set; }
        public DbSet<ISO4217Currency> ISO4217Currency { get; set; }
        public DbSet<TaxDataFileDetails> TaxDataFileDetails { get; set; }

        public TaxDataUploadDB() : base("name=DefaultConnection")
        {

        }

        IQueryable<T> ITaxDataUploadDB.Query<T>()
        {
            return Set<T>();
        }

        void ITaxDataUploadDB.Add<T>(T entity)
        {
            Set<T>().Add(entity);
        }

        void ITaxDataUploadDB.Update<T>(T entity)
        {
            Entry(entity).State = EntityState.Modified;
        }

        void ITaxDataUploadDB.Remove<T>(T entity)
        {
            Set<T>().Remove(entity);
        }

        void ITaxDataUploadDB.SaveChanges()
        {
            SaveChanges();
        }

        void ITaxDataUploadDB.Dispose()
        {
            base.Dispose();
        }

        public void AddRange<T>(IEnumerable<T> entityList) where T : class
        {
            Set<T>().AddRange(entityList);
        }
    }
}