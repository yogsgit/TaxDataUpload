using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using TaxDataUpload.DB;
using TaxDataUpload.Models;

namespace TaxDataUpload.Helpers
{
    public class CacheHelper : ICacheHelper
    {
        ITaxDataUploadDB _db;
        public CacheHelper(ITaxDataUploadDB db)
        {
            _db = db;
        }
        void ICacheHelper.AddISO4217ToCache()
        {
            var ISO4217Data = _db.Query<ISO4217Currency>().ToList();
            HttpContext.Current.Cache.Add("ISO4217Currency", ISO4217Data, null, DateTime.MaxValue, Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
            _db.Dispose();
        }

        bool ICacheHelper.IsCorrectCurrencyCode(string currencyCode)
        {
            var ISO4217Data = (List<ISO4217Currency>)HttpContext.Current.Cache.Get("ISO4217Currency");
            return ISO4217Data.Any(m => m.Code == currencyCode.ToUpper() || m.NumericCode.ToString() == currencyCode);
        }

        List<ISO4217Currency> ICacheHelper.GetISO4217FromCache()
        {
            return (List<ISO4217Currency>)HttpContext.Current.Cache.Get("ISO4217Currency");
        }
    }
}