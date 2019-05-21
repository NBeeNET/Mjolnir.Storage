using NBeeNET.Mjolnir.Storage.Core.Interface;
using NBeeNET.Mjolnir.Storage.Core.Models;
using NBeeNET.Mjolnir.Storage.Print.Serivces;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace NBeeNET.Mjolnir.Storage.Job.Print
{
    /// <summary>
    /// 打印Job
    /// </summary>
    public class PrintJob : IJob
    {
        public string Key { get; set; } = "Print";

        /// <summary>
        /// Job执行函数
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>

        public Task Execute(IJobExecutionContext context)
        {
            var tempFilePath = context.MergedJobDataMap["tempFilePath"].ToString();

            FileInfo fileInfo = new FileInfo(tempFilePath);
            var fileName = fileInfo.Name.Replace(fileInfo.Extension, "");

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                switch (fileInfo.Extension.ToUpper())
                {
                    case ".XLS":
                    case ".XLSX":

                        PrintExcel(context);

                        break;
                    case ".DOC":
                    case ".DOCX":
                    case ".TXT":

                        PrintWord(context);

                        break;
                    case ".PDF":
                        break;
                    default:
                        context.Result = new JsonFileDetail()
                        {
                            State = "-1",
                            Value = "当前操作系统不支持！"
                        };
                        break;
                }
            }
            else
            {
                context.Result = new JsonFileDetail()
                {
                    State = "-1",
                    Value = "当前操作系统不支持！"
                };
            }
            return Task.CompletedTask;
            
        }

        /// <summary>
        /// 打印word
        /// </summary>
        /// <param name="context"></param>
        private void PrintWord(IJobExecutionContext context)
        {
            Microsoft.Office.Interop.Word.Application app = null;
            Microsoft.Office.Interop.Word.Document doc = null;
            var tempFilePath = context.MergedJobDataMap["tempFilePath"].ToString();
            var id = context.MergedJobDataMap["id"].ToString();
            try
            {
                app = new Microsoft.Office.Interop.Word.ApplicationClass();

                app.Visible = false;
                doc = app.Documents.Open(tempFilePath);
                //打印
                object OutFileName = tempFilePath.Replace(".docx", "_print.pdf").Replace(".doc", "_print.pdf");
                doc.PrintOut(OutputFileName: ref OutFileName);
                //等待文件生成
                var jobModel = PrintHandleService.GetResource().GetJobById(id).FirstOrDefault();
                if (jobModel != null)
                {
                    while (jobModel.JobStatus != "打印完成" && jobModel.JobStatus !="打印异常" )
                    {
                        System.Threading.Thread.Sleep(200);
                        jobModel = PrintHandleService.GetResource().GetJobById(id).FirstOrDefault();
                    }
                }
                context.Result = new JsonFileDetail()
                {
                    State = "2",
                    Value = "打印完成"
                };
            }
            catch (Exception ex)
            {
                context.Result = new JsonFileDetail()
                {
                    State = "-1",
                    Value = "打印失败:" + ex.Message
                };
            }
            finally
            {
                if (doc != null)
                    doc.Close();
                if (app != null)
                    app.Quit();
            }
        }
        /// <summary>
        /// 打印excel
        /// </summary>
        /// <param name="path"></param>
        /// <param name="jobModel"></param>
        private void PrintExcel(IJobExecutionContext context)
        {

            Microsoft.Office.Interop.Excel.Application app = null;
            Microsoft.Office.Interop.Excel.Workbook worksBook = null;
            var tempFilePath = context.MergedJobDataMap["tempFilePath"]?.ToString();
            var printName = context.MergedJobDataMap["printName"]?.ToString();//打印机名称
            try
            {
                app = new Microsoft.Office.Interop.Excel.Application(); //声明一个应用程序类实例
                                                                        //ExcelApp.DefaultFilePath = ""; //默认文件路径导出excel的路径还是在参数strFileName里设置
                                                                        //ExcelApp.DisplayAlerts = true;
                                                                        //ExcelApp.SheetsInNewWorkbook = 1;///返回或设置 Microsoft Excel 自动插入到新工作簿中的工作表数目。
                worksBook = app.Workbooks.Open(tempFilePath); //创建一个新工作簿
                Microsoft.Office.Interop.Excel.Worksheet workSheet = (Microsoft.Office.Interop.Excel.Worksheet)worksBook.Worksheets[1]; //在工作簿中得到sheet。
                object outFileName = tempFilePath.Replace(".xlsx", ".pdf").Replace(".xls", ".pdf");
                workSheet.PrintOutEx(PrToFileName: outFileName, ActivePrinter: printName);
                context.Result = new JsonFileDetail()
                {
                    State = "2",
                    Value = "打印完成"
                };
            }
            catch (Exception ex)
            {
                context.Result = new JsonFileDetail()
                {
                    State = "-1",
                    Value = "打印异常:" + ex.ToString()
                };
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
    }

}


