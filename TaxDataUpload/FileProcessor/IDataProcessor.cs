using System.Collections.Generic;
using TaxDataUpload.Models;

namespace TaxDataUpload.FileProcessor
{
    public interface IDataProcessor
    {
        FileProcessorResult ProcessData(List<string> data, string fileName);
    }
}
