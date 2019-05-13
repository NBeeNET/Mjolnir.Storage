using NBeeNET.Mjolnir.Storage.Core.Interface;
using NBeeNET.Mjolnir.Storage.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Office.Jobs
{
    /// <summary>
    /// office文件转pdf
    /// </summary>
    public class TransfPDFJob : IJob
    {
        public JsonFileValues Run(string tempFilePath)
        {
            JsonFileValues job = new JsonFileValues();
            string targetPath = tempFilePath.Replace(".doc", ".pdf");//pdf存放位置;
            FileInfo fileInfo = new FileInfo(tempFilePath);
            job.Key = "Print";
            job.Param = "";
            job.CreateTime = DateTime.Now;
            try
            {
                //var fileName = fileInfo.Name.Replace(fileInfo.Extension, "");
                switch (fileInfo.Extension)
                {
                    case ".xls":
                    case ".xlsx":
                        ExcelToPDF(tempFilePath, targetPath);
                        break;
                    case ".doc":
                    case ".docx":
                        WordToPDF(tempFilePath, targetPath);
                        break;
                }
                job.Status = "1";
                job.Value = Core.StorageOperation.GetUrl(fileInfo.Name);
            }
            catch (Exception ex)
            {
                job.Status = "0";
                job.Value = ex.ToString();
                Console.WriteLine(ex.ToString());
            }
            return job;
        }
        /// <summary>
        /// excel转pdf
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="targetPath"></param>
        private bool ExcelToPDF(string sourcePath, string targetPath)
        {
            Microsoft.Office.Interop.Excel.Application lobjExcelApp = null;
            Microsoft.Office.Interop.Excel.Workbooks lobjExcelWorkBooks = null;
            Microsoft.Office.Interop.Excel.Workbook lobjExcelWorkBook = null;
            string lstrTemp = string.Empty;
            object lobjMissing = System.Reflection.Missing.Value;
            try
            {
                lobjExcelApp = new Microsoft.Office.Interop.Excel.Application();
                lobjExcelApp.Visible = false;
                lobjExcelWorkBooks = lobjExcelApp.Workbooks;
                lobjExcelWorkBook = lobjExcelWorkBooks.Open(sourcePath, true, true, lobjMissing, lobjMissing, lobjMissing, true,
                lobjMissing, lobjMissing, lobjMissing, lobjMissing, lobjMissing, false, lobjMissing, lobjMissing);
                //Microsoft.Office.Interop.Excel 12.0.0.0之后才有这函数           
                lstrTemp = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".xlsx" + (lobjExcelWorkBook.HasVBProject ? 'm' : 'x');
                //输出为PDF 第一个选项指定转出为PDF,还可以指定为XPS格式
                lobjExcelWorkBook.ExportAsFixedFormat(Microsoft.Office.Interop.Excel.XlFixedFormatType.xlTypePDF, targetPath, Microsoft.Office.Interop.Excel.XlFixedFormatQuality.xlQualityStandard, Type.Missing, false, Type.Missing, Type.Missing, false, Type.Missing);
                lobjExcelWorkBooks.Close();
                lobjExcelApp.Quit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;

            }
        }
        /// <summary>
        /// word转pdf
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="targetPath"></param>
        private bool WordToPDF(string sourcePath, string targetPath)
        {
            bool result = false;
            Microsoft.Office.Interop.Word.Application application = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document document = null;
            try
            {
                application.Visible = false;
                document = application.Documents.Open(sourcePath);
                string PDFPath = sourcePath.Replace(".doc", ".pdf");//pdf存放位置
                if (!File.Exists(@PDFPath))//存在PDF，不需要继续转换
                {
                    document.ExportAsFixedFormat(PDFPath, Microsoft.Office.Interop.Word.WdExportFormat.wdExportFormatPDF);
                }
                result = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                result = false;
            }
            finally
            {
                document.Close();
            }
            return result;
        }

    }
}
