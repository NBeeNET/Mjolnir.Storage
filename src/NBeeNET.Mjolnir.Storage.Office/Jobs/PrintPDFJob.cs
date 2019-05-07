using NBeeNET.Mjolnir.Storage.Core.Models;
using Spire.Pdf;
using System;
using System.Collections.Generic;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Office.Jobs
{
    public class PrintPDFJob
    {
        public JsonFileValues Run(string tempFilePath, JsonFileValues job)
        {
            PdfDocument doc = new PdfDocument();

            //Load file
            doc.LoadFromFile(@"D:\PrintPdfDocument.pdf");
            
            doc.Print();
           
            return null;
        }
    }
}
