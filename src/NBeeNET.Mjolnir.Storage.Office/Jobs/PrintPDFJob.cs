using NBeeNET.Mjolnir.Storage.Core.Models;
using Spire.Pdf;
using System;
using System.Collections.Generic;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Office.Jobs
{
    public class PrintPDFJob
    {
        public JsonFileValues Run()
        {
            string filePath = "C:/Users/94885/Desktop/启动指令.pdf";
            PdfDocument doc = new PdfDocument();

            //Load file
            doc.LoadFromFile(filePath);
            
            doc.Print();
           
            return null;
        }
    }
}
