using NBeeNET.Mjolnir.Storage.Print.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NBeeNET.Mjolnir.Storage.Print.Office
{
    public class Excel
    {
        /// <summary>
        /// 同步打印
        /// </summary>
        /// <param name="path"></param>
        /// <param name="msg"></param>
        /// <param name="printerName">打印机名称</param>
        public static void Print(string path, out string msg,string outFileName = null, string printerName = null)
        {
            Microsoft.Office.Interop.Excel.Application app = null;
            Microsoft.Office.Interop.Excel.Workbook worksBook = null;
            string templateFile = path;
            try
            {
                app = new Microsoft.Office.Interop.Excel.Application(); //声明一个应用程序类实例
                worksBook = app.Workbooks.Open(templateFile); //创建一个新工作簿
                Microsoft.Office.Interop.Excel.Worksheet workSheet = (Microsoft.Office.Interop.Excel.Worksheet)worksBook.Worksheets[1]; //在工作簿中得到sheet。
                if (string.IsNullOrEmpty(outFileName))
                    outFileName = path.Replace(".xlsx", ".pdf").Replace(".xls", ".pdf");
                workSheet.PrintOutEx(PrToFileName: outFileName,ActivePrinter: printerName);
                msg = "打印完成";
            }
            catch (Exception exp)
            {
                msg = "打印异常";
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
        }
        /// <summary>
        /// 异步打印
        /// </summary>
        /// <param name="path"></param>
        /// <param name="printerName"></param>
        /// <param name="jobModel"></param>
        public static async void PrintAsync(string path,string outFileName = null, string printerName = null, PrintJobModel jobModel = null)
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
                    if(string.IsNullOrEmpty(outFileName))
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
    }
}
