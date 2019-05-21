﻿using NBeeNET.Mjolnir.Storage.Job.Interface;
using System;
using System.IO;
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

        public async Task Execute(IJobExecutionContext context)
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
                        await Task.Factory.StartNew(() =>
                        {
                            PrintExcel(context);
                        });
                        break;
                    case ".DOC":
                    case ".DOCX":
                    case ".TXT":
                        await Task.Factory.StartNew(() =>
                        {
                            PrintWord(context);
                        });
                        break;
                    case ".PDF":
                        break;
                    default:
                        context.Result = new
                        {
                            State = "-1",
                            Value = "当前操作系统不支持！",
                            CreateTime = DateTime.Now
                        };
                        break;
                }
            }
            else
            {
                context.Result = new
                {
                    State = "-1",
                    Value = "当前操作系统不支持！",
                    CreateTime = DateTime.Now
                };
            }
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
            try
            {
                app = new Microsoft.Office.Interop.Word.ApplicationClass();

                app.Visible = false;
                doc = app.Documents.Open(tempFilePath);
                //打印
                object OutFileName = tempFilePath.Replace(".docx", ".pdf").Replace(".doc", ".pdf");
                doc.PrintOut(OutputFileName: ref OutFileName);

                context.Result = new
                {
                    State = "1",
                    Value = "打印完成",
                    CreateTime = DateTime.Now
                };
            }
            catch(Exception ex)
            {
                context.Result = new
                {
                    State = "-1",
                    Value = "打印失败:" + ex.Message,
                    CreateTime = DateTime.Now
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
        public static async void PrintExcel(IJobExecutionContext context)
        {
            await Task.Run(() =>
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
                    context.Result = new
                    {
                        State = "1",
                        Value = "打印完成",
                        CreateTime = DateTime.Now
                    };
                }
                catch (Exception ex)
                {
                    context.Result = new
                    {
                        State = "-1",
                        Value = "打印异常:" + ex.ToString(),
                        CreateTime = DateTime.Now
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
            });
        }
    }

}

