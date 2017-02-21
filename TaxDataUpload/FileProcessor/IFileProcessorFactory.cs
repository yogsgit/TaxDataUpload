using TaxDataUpload.Models;

namespace TaxDataUpload.FileProcessor
{
    public interface IFileProcessorFactory
    {
        IFileProcessor GetFileProcessor(FileTypeEnum fileType, string fileName);
    }
}
