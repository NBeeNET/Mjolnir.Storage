using NBeeNET.Mjolnir.Storage.Print.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NBeeNET.Mjolnir.Storage.Print.Common
{
    /// <summary>
    /// 打印helper
    /// </summary>
    public class PrintHelper
    {
        /// <summary>
        /// excel打印
        /// </summary>
        /// <param name="path"></param>
        /// <param name="printerName"></param>
        /// <param name="jobModel"></param>
        public static async void ExcelPrintAsync(string path, string outFileName = null, string printerName = null, PrintJobModel jobModel = null)
        {
            await Task.Run(() =>
            {
                if (jobModel == null)
                    jobModel = new PrintJobModel();
                Microsoft.Office.Interop.Excel.Application app = null;
                Microsoft.Office.Interop.Excel.Workbook worksBook = null;
                string templateFile = path;
                try
                {
                    app = new Microsoft.Office.Interop.Excel.Application(); //声明一个应用程序类实例
                    //ExcelApp.DefaultFilePath = ""; //默认文件路径导出excel的路径还是在参数strFileName里设置
                    //ExcelApp.DisplayAlerts = true;
                    //ExcelApp.SheetsInNewWorkbook = 1;///返回或设置 Microsoft Excel 自动插入到新工作簿中的工作表数目。
                    worksBook = app.Workbooks.Open(templateFile); //创建一个新工作簿
                    Microsoft.Office.Interop.Excel.Worksheet workSheet = (Microsoft.Office.Interop.Excel.Worksheet)worksBook.Worksheets[1]; //在工作簿中得到sheet。
                    if (string.IsNullOrEmpty(outFileName))
                        outFileName = path.Replace(".xlsx", ".pdf").Replace(".xls", ".pdf");
                    workSheet.PrintOutEx(PrToFileName: outFileName, ActivePrinter: printerName);
                    jobModel.JobStatus = "打印完成";
                }
                catch (Exception exp)
                {
                    jobModel.JobStatus = "打印异常";
                    throw exp;
                }
                //销毁excel进程
                finally
                {
                    object saveChange = Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges;
                    if (worksBook != null)
                        worksBook.Close();
                    if (app != null)
                        app.Quit();
                }
            });
        }

        /// <summary>
        /// word打印
        /// </summary>
        /// <param name="path"></param>
        /// <param name="jobModel"></param>
        public static async void WordPrintAsync(string path, object outFileName = null, string printerName = null, PrintJobModel jobModel = null)
        {
            await Task.Run(() =>
            {
                if (jobModel == null)
                    jobModel = new PrintJobModel();
                //打印的代码如下：
                Microsoft.Office.Interop.Word.Application app = null;
                Microsoft.Office.Interop.Word.Document doc = null;
                object missing = System.Reflection.Missing.Value;
                object templateFile = path;
                try
                {
                    app = new Microsoft.Office.Interop.Word.ApplicationClass();
                    if (!string.IsNullOrEmpty(printerName))
                        app.ActivePrinter = printerName;
                    doc = app.Documents.OpenNoRepairDialog(ref templateFile, ref missing, ref missing, ref missing);
                    //打印
                    if (string.IsNullOrEmpty(outFileName?.ToString()))
                        outFileName = path.ToLower().Replace(".docx", ".pdf").Replace(".doc", ".pdf");
                    doc.PrintOut(OutputFileName: ref outFileName);
                    jobModel.JobStatus = "打印完成";
                    //doc.PrintOut(ref missing, ref missing, ref missing, ref missing,
                    //     ref missing, ref missing, ref missing, ref missing, ref missing,
                    //     ref missing, ref missing, ref missing, ref missing, ref missing,
                    //     ref missing, ref missing, ref missing, ref missing);
                }
                catch (Exception exp)
                {
                    jobModel.JobStatus = "打印异常";
                    //throw exp;
                }
                //销毁word进程
                finally
                {
                    object saveChange = Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges;
                    if (doc != null)
                        doc.Close(ref saveChange, ref missing, ref missing);
                    if (app != null)
                        app.Quit(ref missing, ref missing, ref missing);
                }
            });
        }
    }
}
