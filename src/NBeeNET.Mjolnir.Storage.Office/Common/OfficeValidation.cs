using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Office.Common
{
    public class OfficeValidation
    {
        /// <summary>
        /// 是否为Office格式
        /// </summary>
        /// <param name="file"></param>
        /// <returns>True(是Office格式)、False(不是Office格式)</returns>
        public static bool IsCheck(IFormFile file)
        {
            byte[] fileBytes;
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                fileBytes = ms.ToArray();
              
            }

            return GetOfficeFormat(fileBytes) != OfficeFormat.unknown;
        }
        private enum OfficeFormat
        {
            pdf,
            doc,
            docx,
            xls,
            xlsx,
            ppt,
            pptx,
            unknown
        }

        private static OfficeFormat GetOfficeFormat(byte[] bytes)
        {
            var formateByte= bytes[0].ToString() + bytes[1].ToString();
            var pdf=new byte[] { 37, 80, 68, 70, 45, 49, 46, 52 };
            var docx = new byte[] { 80, 75, 3, 4, 10, 0, 6, 0, 8, 0, 0, 0, 33 };
            var xlsx=new byte[] { 80, 75, 3, 4, 20, 0, 6, 0, 8, 0, 0, 0, 33 };
            var pptx = new byte[] { 80, 75, 3, 4, 20, 0, 6, 0, 8, 0, 0, 0, 33 };
            var doc = new byte[] { 208, 207, 17, 224, 161, 177, 26, 225 }; 
            var xls = new byte[] { 208, 207, 17, 224, 161, 177, 26, 225 }; 
            var ppt = new byte[] { 208, 207, 17, 224, 161, 177, 26, 225 }; 

            if (pdf.SequenceEqual(bytes.Take(pdf.Length)))
                return OfficeFormat.pdf;

            if (docx.SequenceEqual(bytes.Take(docx.Length)))
                return OfficeFormat.docx;

            if (xlsx.SequenceEqual(bytes.Take(xlsx.Length)))
                return OfficeFormat.xlsx;

            if (pptx.SequenceEqual(bytes.Take(pptx.Length)))
                return OfficeFormat.pptx;

            if (doc.SequenceEqual(bytes.Take(doc.Length)))
                return OfficeFormat.doc;

            if (xls.SequenceEqual(bytes.Take(xls.Length)))
                return OfficeFormat.xls;

            if (ppt.SequenceEqual(bytes.Take(ppt.Length)))
                return OfficeFormat.ppt;

            return OfficeFormat.unknown;
        }
    }
}
