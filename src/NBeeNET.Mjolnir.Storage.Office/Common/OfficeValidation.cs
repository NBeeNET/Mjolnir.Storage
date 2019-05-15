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
            var format = file.FileName.Split(".")[file.FileName.Split(".").Length - 1];

            var obj = new object();
            return Enum.TryParse(typeof(OfficeFormat), format,out obj);
        }

        enum OfficeFormat
        {
            pdf,
            doc,
            docx,
            xls,
            xlsx,
            ppt,
            pptx
        }

    }
}
