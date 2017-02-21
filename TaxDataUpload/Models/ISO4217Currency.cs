using System.ComponentModel.DataAnnotations;

namespace TaxDataUpload.Models
{
    public class ISO4217Currency
    {
        public int Id { get; set; }
        [MaxLength(200)]
        public string Country { get; set; }
        [MaxLength(200)]
        public string Currency { get; set; }
        [MaxLength(3)]
        public string Code { get; set; }
        public int? NumericCode { get; set; }
        public int? MinorUnits { get; set; }
    }
}