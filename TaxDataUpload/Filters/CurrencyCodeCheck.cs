using System.ComponentModel.DataAnnotations;
using TaxDataUpload.Helpers;

namespace TaxDataUpload.Filters
{
    public class CurrencyCodeCheck : ValidationAttribute
    {
        ICacheHelper cacheHelper = new CacheHelper(null);
        public override bool IsValid(object value)
        {
            return cacheHelper.IsCorrectCurrencyCode(value.ToString());
        }
    }
}