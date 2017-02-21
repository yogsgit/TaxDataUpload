using System.Collections.Generic;

namespace TaxDataUpload.Models
{
    public class FileProcessorResult
    {
        private readonly string error;
        private readonly string fileName;
        private readonly int totalRecords;
        private readonly int savedRecords;
        private readonly int rejectedRecords;
        private readonly List<TransactionData> rejectedData;

        public string Error { get { return error; } }
        public string FileName { get { return fileName; } }
        public int TotalRecords { get { return totalRecords; } }
        public int SavedRecords { get { return savedRecords; } }
        public int RejectedRecords { get { return rejectedRecords; } }
        public List<TransactionData> RejectedData { get { return rejectedData; } }

        public FileProcessorResult(string error = null, string fileName = null, int totalRecords = 0, int savedRecords = 0, 
            int rejectedRecords = 0, List<TransactionData> rejectedData = null)
        {
            this.error = error;
            this.fileName = fileName;
            this.totalRecords = totalRecords;
            this.savedRecords = savedRecords;
            this.rejectedRecords = rejectedRecords;
            this.rejectedData = rejectedData;
        }
    }
}