using System.Collections.Generic;
using TaxDataUpload.Models;

namespace TaxDataUpload.Helpers
{
    public interface ICacheHelper
    {
        void AddISO4217ToCache();

        bool IsCorrectCurrencyCode(string currencyCode);

        List<ISO4217Currency> GetISO4217FromCache();
    }
}
