using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using TaxDataUpload.DB;
using TaxDataUpload.Helpers;
using Moq;
using TaxDataUpload.Models;
using TaxDataUpload.FileProcessor;

namespace TaxDataUpload.Tests
{
    [TestClass]
    public class DataProcessorTests
    {
        static Mock<ITaxDataUploadDB> mockITaxDataUploadDB;
        static Mock<ICacheHelper> mockICacheHelper;

        [ClassInitialize()]
        public static void ClassInit(TestContext context)
        {
            mockITaxDataUploadDB = new Mock<ITaxDataUploadDB>();
            mockITaxDataUploadDB.Setup(m => m.Query<TaxDataFileDetails>())
                .Returns(new List<TaxDataFileDetails>() { new TaxDataFileDetails() { FileName = "test.csv", Id = 1, UploadTime = DateTime.UtcNow } }.AsQueryable());

            mockICacheHelper = new Mock<ICacheHelper>();
            mockICacheHelper.Setup(m => m.GetISO4217FromCache())
                .Returns(TestHelper.GetMockISO4217List());
        }

        [TestMethod]
        public void DataProcessor_1Correct()
        {
            IDataProcessor dataProcessor = new DataProcessor(mockITaxDataUploadDB.Object, mockICacheHelper.Object);
            var result = dataProcessor.ProcessData(new List<string>() { "account,description,INR,100" }, "test.csv");

            Assert.IsNotNull(result);
            Assert.AreEqual(result.TotalRecords, 1);
            Assert.AreEqual(result.SavedRecords, 1);
            Assert.AreEqual(result.RejectedRecords, 0);
            Assert.AreEqual(result.RejectedData.Count, 0);
        }

        [TestMethod]
        public void DataProcessor_1Incorrect()
        {
            IDataProcessor dataProcessor = new DataProcessor(mockITaxDataUploadDB.Object, mockICacheHelper.Object);
            var result = dataProcessor.ProcessData(new List<string>() { "account,description,USD,100" }, "test.csv");

            Assert.IsNotNull(result);
            Assert.AreEqual(result.TotalRecords, 1);
            Assert.AreEqual(result.SavedRecords, 0);
            Assert.AreEqual(result.RejectedRecords, 1);
            Assert.AreEqual(result.RejectedData.Count, 1);
        }

        [TestMethod]
        public void DataProcessor_1Correct_1Incorrect()
        {
            IDataProcessor dataProcessor = new DataProcessor(mockITaxDataUploadDB.Object, mockICacheHelper.Object);
            var result = dataProcessor.ProcessData(new List<string>() { "account,description,INR,100", "account,description,USD,100" }, "test.csv");

            Assert.IsNotNull(result);
            Assert.AreEqual(result.TotalRecords, 2);
            Assert.AreEqual(result.SavedRecords, 1);
            Assert.AreEqual(result.RejectedRecords, 1);
            Assert.AreEqual(result.RejectedData.Count, 1);
        }
    }
}
