using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TaxDataUpload.FileProcessor
{
    public class CSVFileProcessor : IFileProcessor
    {
        private readonly string fileName;
        public CSVFileProcessor(string fileName)
        {
            this.fileName = fileName;
        }
        public IEnumerable<string> ProcessTaxDataFile()
        {
            var fileData = File.ReadAllLines(fileName);
            if (fileData.Count() <= 1)
                return new string[] { "Error: Tax data file has no data." };
            else if (fileData[0].ToLower().Trim() != "account,description,currency code,amount")
                return new string[] { "Error: Tax data file has invalid data." };
            else
                return fileData.Skip(1).Where(m => !string.IsNullOrEmpty(m) && !string.IsNullOrWhiteSpace(m) && (m.ToCharArray().Any(n => n != ',' && n != ' ' && n != '\t')));
        }
    }
}