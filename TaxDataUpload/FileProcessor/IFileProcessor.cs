using System.Collections.Generic;

namespace TaxDataUpload.FileProcessor
{
    public interface IFileProcessor
    {
        IEnumerable<string> ProcessTaxDataFile();
    }
}
