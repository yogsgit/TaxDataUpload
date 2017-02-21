using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaxDataUpload.FileProcessor;
using TaxDataUpload.Filters;
using TaxDataUpload.Models;

namespace TaxDataUpload.Controllers
{
    public class UploadController : Controller
    {
        private IDataProcessor dataProcessor;
        private IFileProcessorFactory fileProcesorFactory;

        public UploadController(IDataProcessor dataProcessor, IFileProcessorFactory fileProcesorFactory)
        {
            this.dataProcessor = dataProcessor;
            this.fileProcesorFactory = fileProcesorFactory;
            
        }
        // GET: Upload
        public ActionResult Index(string err, string fil)
        {
            if (string.IsNullOrEmpty(err))
                ViewBag.IsPost = false;
            else
            {
                ViewBag.IsPost = true;
                ViewBag.Error = err;
                ViewBag.FileName = fil;
            }
            return View();
        }

        // POST: Upload/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [FileTypeFilter]
        public ActionResult Index(HttpPostedFileBase file)
        {
            string path = Path.Combine(Server.MapPath("Temp"), Path.GetFileName(file.FileName));
            try
            {
                ViewBag.IsPost = true;
                ViewBag.Error = "";
                ViewBag.FileName = Path.GetFileName(file.FileName);
                FileTypeEnum fileType = file.FileName.ToLower().EndsWith(".csv") ? FileTypeEnum.CSV : FileTypeEnum.XLSX;
                file.SaveAs(path);
                var fileProcessor = fileProcesorFactory.GetFileProcessor(fileType, path);
                var fileData = fileProcessor.ProcessTaxDataFile().ToList();
                if (!fileData[0].StartsWith("Error"))
                {
                    var result = dataProcessor.ProcessData(fileData, Path.GetFileName(file.FileName));
                    ViewBag.TotalRecords = result.TotalRecords.ToString();
                    ViewBag.TotalSaved = result.SavedRecords.ToString();
                    ViewBag.TotalFailed = result.RejectedRecords.ToString();
                    return View(result.RejectedData);
                }
                else
                {
                    ViewBag.Error = fileData[0];
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error: Something went wrong while processing the file. Please try again.";
            }
            finally
            {
                System.IO.File.Delete(path);
            }
            
            return View();
        }
    }
}
