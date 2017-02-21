using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaxDataUpload.FileProcessor;

namespace TaxDataUpload.Tests
{
    [TestClass]
    public class FileProcessorFactoryTests
    {
        [TestMethod]
        public void FileProcessorFactory_CSV()
        {
            IFileProcessorFactory factory = new FileProcessorFactory();
            IFileProcessor processor = factory.GetFileProcessor(Models.FileTypeEnum.CSV, "");

            Assert.IsNotNull(processor);
            Assert.IsTrue(processor is CSVFileProcessor);
        }

        [TestMethod]
        public void FileProcessorFactory_XLSX()
        {
            IFileProcessorFactory factory = new FileProcessorFactory();
            IFileProcessor processor = factory.GetFileProcessor(Models.FileTypeEnum.XLSX, "");

            Assert.IsNotNull(processor);
            Assert.IsTrue(processor is XLSXFileProcessor);
        }
    }
}
