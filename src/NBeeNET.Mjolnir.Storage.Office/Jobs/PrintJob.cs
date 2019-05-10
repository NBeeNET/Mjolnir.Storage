using NBeeNET.Mjolnir.Storage.Core.Interface;
using NBeeNET.Mjolnir.Storage.Core.Models;
using Spire.Doc;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

namespace NBeeNET.Mjolnir.Storage.Office.Jobs
{
    /// <summary>
    /// 打印job
    /// </summary>
    public class PrintJob : IJob
    {
        public JsonFileValues Run(string tempFilePath)
        {
            Console.WriteLine("开始打印");
            PrintDocument fPrintDocument = new PrintDocument();    //获取默认打印机的方法
            Console.WriteLine("默认打印机:" + fPrintDocument.PrinterSettings.PrinterName);
            Console.WriteLine("打印文件路径:" + tempFilePath);
            JsonFileValues job = new JsonFileValues();
            FileInfo fileInfo = new FileInfo(tempFilePath);
            try
            {
                //var fileName = fileInfo.Name.Replace(fileInfo.Extension, "");
                switch (fileInfo.Extension)
                {
                    case ".xls":
                    case ".xlsx":
                        PrintExcel(tempFilePath);
                        break;
                    case ".doc":
                    case ".docx":
                        PrintDoc(tempFilePath);
                        break;
                    case ".pdf":
                        PrintPDF(tempFilePath);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            job.Key = "Print";
            job.Param = "";
            job.Status = "1";
            job.Value = Core.StorageOperation.GetUrl(fileInfo.Name);
            job.CreateTime = DateTime.Now;
            Console.WriteLine("结束打印");
            return job;
        }
        /// <summary>
        /// 打印excel
        /// </summary>
        /// <param name="filePath"></param>
        public void PrintExcel(string filePath)
        {
            Workbook workbook = new Workbook();
            workbook.LoadFromFile(filePath);
            workbook.PrintDocument.Print();
            workbook.Dispose();
        }
        /// <summary>
        /// 打印word
        /// </summary>
        /// <param name="filePath"></param>
        public void PrintDoc(string filePath)
        {
            Document document = new Document();
            document.LoadFromFile(filePath);
            document.PrintDocument.Print();
            document.Close();
        }
        /// <summary>
        /// 打印pdf
        /// </summary>
        /// <param name="filePath"></param>
        public void PrintPDF(string filePath)
        {
            //PrintDocument    
            //Create a pdf document.
            //PdfDocument doc = new PdfDocument();
            //doc.LoadFromFile(filePath);
            //doc.Print();
            //doc.Close();
            using (var proc = CreateProcess(filePath))
            {
                proc.Start();
                bool result = proc.WaitForExit(1000);
                if (!result)
                {
                    proc.Kill();
                }
            }
        }

        private static readonly string utilPath = GetUtilPath();
        
        private static Process CreateProcess(string filePath)
        {
            return new Process
            {
                StartInfo =
                    {
                    WindowStyle = ProcessWindowStyle.Hidden,
                        FileName = utilPath,
                        Arguments = $@"""{filePath}""",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
            };
        }

        private static string GetUtilPath()
        {
            return Path.Combine(
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                        "PDFtoPrinter.exe");
        }
    }
}

