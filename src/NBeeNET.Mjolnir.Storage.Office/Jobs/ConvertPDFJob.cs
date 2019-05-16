using NBeeNET.Mjolnir.Storage.Core.Interface;
using NBeeNET.Mjolnir.Storage.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace NBeeNET.Mjolnir.Storage.Office.Jobs
{
    public class ConvertPDFJob
    {
        public JsonFileValues Run(string tempFilePath, JsonFileValues job)
        {
            FileInfo fileInfo = new FileInfo(tempFilePath);
            switch (fileInfo.Extension.ToUpper())
            {
                case ".xls":
                case ".xlsx":
                    break;
                case ".DOC":
                case ".DOCX":
                    job = WordToPDF(tempFilePath, job);
                    break;
                case ".pdf":
                    break;
            }
            return job;
        }

        private static JsonFileValues WordToPDF(string tempFilePath, JsonFileValues job)
        {
            FileInfo fileInfo = new FileInfo(tempFilePath);
            var fileName = fileInfo.Name.Replace(fileInfo.Extension, "");
            string pdfName = string.Format("{0}{1}", fileName, ".pdf");
            string pdfPath = fileInfo.DirectoryName + "\\" + pdfName;

            Microsoft.Office.Interop.Word.Application application = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document document = null;

            application.Visible = false;
            document = application.Documents.Open(tempFilePath);
            if (!File.Exists(pdfPath))
                document.ExportAsFixedFormat(pdfPath, Microsoft.Office.Interop.Word.WdExportFormat.wdExportFormatPDF);

            document.Close();
            application.Quit();

            job.Status = "1";
            job.Value = Core.StorageOperation.GetUrl(pdfName);
            job.CreateTime = DateTime.Now;

            return job;
        }
    }
}
