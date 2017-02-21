using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaxDataUpload.Filters
{
    public class FileTypeFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var input = filterContext.ActionParameters["file"];
            string error = null;

            if (input == null || ((HttpPostedFileBase)input).ContentLength == 0)
            {
                error = "Error: Please select a non-empty data file.";
                if (input == null)
                    filterContext.Result = new RedirectResult("/upload/index?err=" + error);
                else
                    filterContext.Result = new RedirectResult("/upload/index?err=" + error + "&fil=" + Path.GetFileName(((HttpPostedFileBase)input).FileName));
            }
            else
            {
                var csvTypes = new string[] { "application/csv", "text/csv", "application/vnd.ms-excel", "application/vnd.ms-excel" };
                var xlsxTypes = new string[] { "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" };

                var file = (HttpPostedFileBase)input;
                if (!((file.FileName.ToLower().EndsWith(".csv") && csvTypes.Any(m => m == file.ContentType.ToLower())) 
                    || (file.FileName.ToLower().EndsWith(".xlsx") && xlsxTypes.Any(m => m == file.ContentType.ToLower()))))
                {
                    error = "Error: Invalid file type. Please select a valid .csv or .xlsx file.";
                    filterContext.Result = new RedirectResult("/upload/index?err=" + error + "&fil=" + Path.GetFileName(file.FileName));
                }
            }
        }
    }
}