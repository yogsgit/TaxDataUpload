using System.Collections.Generic;
using TaxDataUpload.Models;

namespace TaxDataUpload.Tests
{
    public class TestHelper
    {
        public static List<ISO4217Currency> GetMockISO4217List()
        {
            return new List<ISO4217Currency>()
            {
                new ISO4217Currency() { Country = "UK", Currency="Pound Sterling", Code="GBP", Id=0,NumericCode=100, MinorUnits=2},
                new ISO4217Currency() { Country = "India", Currency="Indian Rupee", Code="INR", Id=0, NumericCode=101, MinorUnits=2 }
            };
        }
    }
}
