using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaxDataUpload.DB;
using TaxDataUpload.Helpers;
using TaxDataUpload.Models;

namespace TaxDataUpload.FileProcessor
{
    public class DataProcessor : IDataProcessor
    {
        private readonly ITaxDataUploadDB _db;
        private readonly ICacheHelper _cacheHelper;
        public DataProcessor(ITaxDataUploadDB db, ICacheHelper cacheHelper)
        {
            _db = db;
            _cacheHelper = cacheHelper;
        }
        public FileProcessorResult ProcessData(List<string> data, string fileName)
        {
            _db.Add<TaxDataFileDetails>(new TaxDataFileDetails() { UploadTime = DateTime.UtcNow, FileName = fileName });
            _db.SaveChanges();
            var fileDetialsId = _db.Query<TaxDataFileDetails>().Max(m => m.Id);
            var errorData = new List<TransactionData>();
            int errorCnt = 0;
            var ISO4217Data = _cacheHelper.GetISO4217FromCache();
            var processedData = new List<TransactionData>();
            
            Parallel.ForEach(data, (currentData) =>
            {
                var values = currentData.Split(new char[] { ',' });
                TransactionData row = null;
                switch (values.Count())
                {
                    case 1:
                        row = new TransactionData(values[0], null, null, null, ISO4217Data);
                        break;
                    case 2:
                        row = new TransactionData(values[0], values[1], null, null, ISO4217Data);
                        break;
                    case 3:
                        row = new TransactionData(values[0], values[1], values[2], null , ISO4217Data);
                        break;
                    case 4:
                        row = new TransactionData(values[0], values[1], values[2], values[3], ISO4217Data);
                        break;
                }
                if (row.Errors.Count == 0)
                {
                    row.TaxDataFileDetailsId = fileDetialsId;
                    bool lockWasTaken = false;
                    try
                    {
                        Monitor.Enter(processedData, ref lockWasTaken);
                        processedData.Add(row);
                    }
                    finally
                    {
                        if (lockWasTaken)
                        {
                            Monitor.Exit(processedData);
                        }
                    }
                }
                else
                {
                    bool lockWasTaken = false;
                    try
                    {
                        Monitor.Enter(errorData, ref lockWasTaken);
                        errorData.Add(row);
                        Interlocked.Increment(ref errorCnt);
                    }
                    finally
                    {
                        if (lockWasTaken)
                        {
                            Monitor.Exit(errorData);
                        }
                    }
                }
            });
            _db.AddRange<TransactionData>(processedData);
            _db.SaveChanges();
            _db.Dispose();
            return new FileProcessorResult(null, null, data.Count(), data.Count() - errorCnt, errorCnt, errorData);
        }
    }
}