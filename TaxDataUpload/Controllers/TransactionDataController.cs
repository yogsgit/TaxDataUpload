using PagedList;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TaxDataUpload.DB;
using TaxDataUpload.Models;

namespace TaxDataUpload.Controllers
{
    public class TransactionDataController : Controller
    {
        private ITaxDataUploadDB _db;

        public TransactionDataController(ITaxDataUploadDB db)
        {
            _db = db;
        }

        // GET: TransactionData
        public ActionResult Index(int? page)
        {
            var data = _db.Query<TransactionData>();

            var pageNumber = page ?? 1;
            var pageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["TransactionDataPageSize"]);
            var onePageOfData = data.Include(t => t.TaxDataFileDetails).OrderBy(m => m.Id).ToPagedList(pageNumber, pageSize);

            ViewBag.OnePageOfData = onePageOfData;
            return View(onePageOfData.ToList());
        }

        // GET: TransactionData/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransactionData transactionData = _db.Query<TransactionData>().Include(t => t.TaxDataFileDetails).Where(m => m.Id == id).FirstOrDefault();
            if (transactionData == null)
            {
                return HttpNotFound();
            }
            return View(transactionData);
        }

        // POST: TransactionData/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Account,Description,CurrencyCode,Amount,TaxDataFileDetailsId")] TransactionData transactionData)
        {
            if (ModelState.IsValid)
            {
                _db.Update<TransactionData>(transactionData);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(transactionData);
        }

        // GET: TransactionData/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransactionData transactionData = _db.Query<TransactionData>().Include(t => t.TaxDataFileDetails).Where(m => m.Id == id).FirstOrDefault();
            if (transactionData == null)
            {
                return HttpNotFound();
            }
            return View(transactionData);
        }

        // POST: TransactionData/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TransactionData transactionData = _db.Query<TransactionData>().Where(m => m.Id == id).FirstOrDefault();
            _db.Remove<TransactionData>(transactionData);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
