using TaxDataUpload.Models;

namespace TaxDataUpload.FileProcessor
{
    public class FileProcessorFactory : IFileProcessorFactory
    {
        public IFileProcessor GetFileProcessor(FileTypeEnum fileType, string fileName)
        {
            switch(fileType)
            {
                case FileTypeEnum.CSV:
                    return new CSVFileProcessor(fileName);
                case FileTypeEnum.XLSX:
                    return new XLSXFileProcessor(fileName);
                default:
                    return null;
            }
        }
    }
}