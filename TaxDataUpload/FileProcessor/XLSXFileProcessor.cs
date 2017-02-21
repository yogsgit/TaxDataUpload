using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TaxDataUpload.FileProcessor
{
    public class XLSXFileProcessor : IFileProcessor
    {
        private readonly string fileName;
        public XLSXFileProcessor(string fileName)
        {
            this.fileName = fileName;
        }
        public IEnumerable<string> ProcessTaxDataFile()
        {
            List<string> data = new List<string>();
            using (var fileStream = new FileStream(fileName, FileMode.Open))
            {
                try
                {
                    using (var package = new ExcelPackage(fileStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;
                        if (noOfCol < 4 || noOfRow <= 1)
                            data.Add("Error: Tax data file has no data.");
                        else
                        {
                            StringBuilder header = new StringBuilder();
                            header.Append(workSheet.Cells[1, 1].Value == null ? string.Empty : workSheet.Cells[1, 1].Value.ToString());
                            header.Append(",");
                            header.Append(workSheet.Cells[1, 2].Value == null ? string.Empty : workSheet.Cells[1, 2].Value.ToString());
                            header.Append(",");
                            header.Append(workSheet.Cells[1, 3].Value == null ? string.Empty : workSheet.Cells[1, 3].Value.ToString());
                            header.Append(",");
                            header.Append(workSheet.Cells[1, 4].Value == null ? string.Empty : workSheet.Cells[1, 4].Value.ToString());
                            if (header.ToString().ToLower() != "account,description,currency code,amount")
                                data.Add("Error: Tax data file has invalid data.");
                            else
                            {
                                StringBuilder dataRow = new StringBuilder();
                                for (int i = 2; i <= noOfRow; i++)
                                {
                                    dataRow.Append(workSheet.Cells[i, 1].Value == null ? string.Empty : workSheet.Cells[i, 1].Value.ToString());
                                    dataRow.Append(",");
                                    dataRow.Append(workSheet.Cells[i, 2].Value == null ? string.Empty : workSheet.Cells[i, 2].Value.ToString());
                                    dataRow.Append(",");
                                    dataRow.Append(workSheet.Cells[i, 3].Value == null ? string.Empty : workSheet.Cells[i, 3].Value.ToString());
                                    dataRow.Append(",");
                                    dataRow.Append(workSheet.Cells[i, 4].Value == null ? string.Empty : workSheet.Cells[i, 4].Value.ToString());
                                    if (!(dataRow.Length == 3))
                                        data.Add(dataRow.ToString());
                                    dataRow.Clear();
                                }
                            }
                        }
                        package.Dispose();
                    }
                }
                finally
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }
            return data;
        }
    }
}