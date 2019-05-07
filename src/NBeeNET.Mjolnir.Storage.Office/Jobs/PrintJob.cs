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
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Image.Jobs
{

    public class PrintJob
    {
        public JsonFileValues Run(string tempFilePath, JsonFileValues job)
        {
            FileInfo fileInfo = new FileInfo(tempFilePath);
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

            return job;
        }

        public void PrintExcel(string filePath)
        {
            Workbook workbook = new Workbook();
            workbook.LoadFromFile(filePath);
            workbook.PrintDocument.Print();
            workbook.Dispose();
        }

        public void PrintDoc(string filePath)
        {
            Document document = new Document();
            document.LoadFromFile(filePath);
            document.PrintDocument.Print();
            document.Close();
        }
        public void PrintPDF(string filePath)
        {
            filePath = "C:/Users/94885/Desktop/启动指令.pdf";
            //Create a pdf document.
            PdfDocument doc = new PdfDocument();
            doc.LoadFromFile(filePath);
            doc.Print();
            doc.Close();
        }
    }
}

