using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaxDataUpload.Models
{
    public class TaxDataFileDetails
    {
        public int Id { get; set; }
        [Display(Name = "Upload Time")]
        public DateTime UploadTime { get; set; }
        [MaxLength(300)]
        [Display(Name = "File Name")]
        public string FileName { get; set; }

        public virtual ICollection<TransactionData> TransactionData { get; set; }
        
    }
}