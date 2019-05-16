using NBeeNET.Mjolnir.Storage.Core.Interface;
using NBeeNET.Mjolnir.Storage.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NBeeNET.Mjolnir.Storage.Office.Jobs
{
    public class CreatePDFJob
    {
        public JsonFileDetail Run(string tempFilePath, JsonFileDetail job)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                FileInfo fileInfo = new FileInfo(tempFilePath);
                switch (fileInfo.Extension.ToUpper())
                {
                    case ".XLS":
                    case ".XLSX":
                        break;
                    case ".DOC":
                    case ".DOCX":
                    case ".TXT":
                        job = WordToPDF(tempFilePath, job);
                        break;
                    case ".PDF":
                        break;
                    default:
                        job.State = "-1";
                        job.Value = "当前不支持！";
                        break;
                }
            }
            else
            {
                job.State = "-1";
                job.Value = "当前操作系统不支持！";
            }
            return job;
        }

        private static JsonFileDetail WordToPDF(string tempFilePath, JsonFileDetail job)
        {
            Microsoft.Office.Interop.Word.Application application = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document document = null;

            try
            {
                FileInfo fileInfo = new FileInfo(tempFilePath);
                var fileName = fileInfo.Name.Replace(fileInfo.Extension, "");
                string pdfName = string.Format("{0}{1}", fileName, ".pdf");
                string pdfPath = fileInfo.DirectoryName + "\\" + pdfName;

                application.Visible = false;
                document = application.Documents.Open(tempFilePath);
                if (!File.Exists(pdfPath))
                    document.ExportAsFixedFormat(pdfPath, Microsoft.Office.Interop.Word.WdExportFormat.wdExportFormatPDF);

                job.State = "1";
                job.Value = Core.StorageOperation.GetUrl(pdfName);
                job.CreateTime = DateTime.Now;
            }
            catch (Exception)
            {
                job.State = "-1";
                job.Value = "生成失败";
                job.CreateTime = DateTime.Now;
            }
            finally
            {
                document.Close();
                application.Quit();
            }
            
            return job;
        }
    }
}
