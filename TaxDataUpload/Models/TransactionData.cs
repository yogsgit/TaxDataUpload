using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using TaxDataUpload.Filters;

namespace TaxDataUpload.Models
{
    public class TransactionData
    {
        private string account;
        private string description;
        private string currencyCode;
        private decimal amount;
        private string amountString;

        public TransactionData()
        {

        }

        public TransactionData(string account, string description, string currencyCode, string amount, List<ISO4217Currency> ISO4217Data)
        {
            if (string.IsNullOrEmpty(account))
                errors.Add("Account is missing.");
            this.account = account;
            if (string.IsNullOrEmpty(description))
                errors.Add("Description is missing.");
            this.description = description;
            if (string.IsNullOrEmpty(currencyCode))
                errors.Add("Currency code is missing.");
            else if (currencyCode.Length != 3 || !ISO4217Data.Any(m => m.Code == currencyCode.ToUpper() || m.NumericCode.ToString() == currencyCode))
                errors.Add("Invalid currency code.");
            this.currencyCode = currencyCode;
            decimal amt = -0.0m;
            if (string.IsNullOrEmpty(amount))
                errors.Add("Amount is missing.");
            else if (!decimal.TryParse(amount, out amt) || amt <= 0)
                errors.Add("Invalid amount.");
            this.amount = amt;
            amountString = amount;
        }

        public int Id { get ; set ; }
        [Required(ErrorMessage = "Please enter an Account.")]
        public string Account
        {
            get { return account; }
            set { account = value; }
        }

        [Required(ErrorMessage = "Please enter a Description.")]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        [Required(ErrorMessage = "Please enter a valid ISO 4217 Currency Code.")]
        [MaxLength(3, ErrorMessage = "Please enter a valid ISO 4217 Currency Code of length 3.")]
        [Display(Name = "Currency Code")]
        [CurrencyCodeCheck(ErrorMessage = "Please enter a correct Currency Code.")]
        public string CurrencyCode
        {
            get { return currencyCode; }
            set { currencyCode = value; }
        }

        [Required(ErrorMessage = "Please enter an Amount.")]
        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        private List<string> errors = new List<string>();
        [NotMapped]
        public List<string> Errors { get { return errors; } set { errors = value; } }
        [NotMapped]
        public string AmountString { get { return amountString; } }
        [Required(ErrorMessage = "Tax data field details have been tampered with.")]
        public int TaxDataFileDetailsId { get; set; }

        public virtual TaxDataFileDetails TaxDataFileDetails
        {
            get; set;
        }
    }
}