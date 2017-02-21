using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using TaxDataUpload.Controllers;

namespace TaxDataUpload.Tests.Controllers
{
    [TestClass]
    public class UploadControllerTest
    {
        [TestMethod]
        public void Index_NoParameters()
        {
            UploadController controller = new UploadController(null, null);

            ViewResult result = controller.Index("", "") as ViewResult;

            Assert.IsFalse(result.ViewBag.IsPost);
        }

        [TestMethod]
        public void Index_WithParameters()
        {
            UploadController controller = new UploadController(null, null);

            ViewResult result = controller.Index("error", "test.csv") as ViewResult;

            Assert.IsTrue(result.ViewBag.IsPost);
            Assert.AreEqual(result.ViewBag.Error, "error");
            Assert.AreEqual(result.ViewBag.FileName, "test.csv");
        }
    }
}
