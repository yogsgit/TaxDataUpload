using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TaxDataUpload.Models;

namespace TaxDataUpload.Tests
{
    [TestClass]
    public class TransactionDataTests
    {
        static List<ISO4217Currency> ISO4217Mock;
        [ClassInitialize()]
        public static void ClassInit(TestContext context)
        {
            ISO4217Mock = TestHelper.GetMockISO4217List();
        }

        [TestMethod]
        public void TransactionData_AllNull()
        {
            TransactionData data = new TransactionData(null, null, null, null, ISO4217Mock);
            Assert.IsNotNull(data);
            Assert.AreEqual(data.Errors.Count, 4);
        }

        [TestMethod]
        public void TransactionData_CurrencyCodeIncorrect()
        {
            TransactionData data = new TransactionData("account", "description", "USD", "100", ISO4217Mock);
            Assert.IsNotNull(data);
            Assert.AreEqual(data.Errors.Count, 1);
            Assert.AreEqual(data.Errors[0], "Invalid currency code.");
        }

        [TestMethod]
        public void TransactionData_AmountString()
        {
            TransactionData data = new TransactionData("account", "description", "INR", "amount", ISO4217Mock);
            Assert.IsNotNull(data);
            Assert.AreEqual(data.Errors.Count, 1);
            Assert.AreEqual(data.Errors[0], "Invalid amount.");
        }

        [TestMethod]
        public void TransactionData_Amount0()
        {
            TransactionData data = new TransactionData("account", "description", "INR", "0", ISO4217Mock);
            Assert.IsNotNull(data);
            Assert.AreEqual(data.Errors.Count, 1);
            Assert.AreEqual(data.Errors[0], "Invalid amount.");
        }

        [TestMethod]
        public void TransactionData_AllCorrect()
        {
            TransactionData data = new TransactionData("account", "description", "INR", "100", ISO4217Mock);
            Assert.IsNotNull(data);
            Assert.AreEqual(data.Errors.Count, 0);
            Assert.AreEqual(data.AmountString, "100");
        }
    }
}
