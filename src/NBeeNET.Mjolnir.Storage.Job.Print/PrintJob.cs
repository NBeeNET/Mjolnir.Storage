using NBeeNET.Mjolnir.Storage.Core.Interface;
using NBeeNET.Mjolnir.Storage.Core.Models;
using NBeeNET.Mjolnir.Storage.Print.Serivces;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Drawing.Printing;
using System.Diagnostics;
using System.Collections.Specialized;
using System.Reflection;
using System.Drawing;

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
                        PrintPDF(context);
                        break;
                    case ".PPT":
                    case ".PPTX":
                        PrintPPT(context);
                        break;
                    case ".JPG":
                        PrintImage(context);
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
        private void PrintImage(IJobExecutionContext context)
        {
            var tempFilePath = context.MergedJobDataMap["tempFilePath"].ToString();
            var id = context.MergedJobDataMap["id"].ToString();
            try
            {
                image = new Bitmap(tempFilePath);
                PrintDocument printDocument = new PrintDocument();
                printDocument.DocumentName = id;
                printDocument.PrintPage += PrintDocument_PrintPage;
                printDocument.Print();
                //等待文件生成
                var jobModel = PrintHandleService.GetResource().GetJobById(id).FirstOrDefault();
                if (jobModel != null)
                {
                    while (jobModel.JobStatus != "打印完成" && jobModel.JobStatus != "打印异常")
                    {
                        System.Threading.Thread.Sleep(200);
                        jobModel = PrintHandleService.GetResource().GetJobById(id).FirstOrDefault();
                    }
                }
                image.Dispose();
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

            }
        }

        private Image image= null;
        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (image != null)
            {
                e.Graphics.DrawImage(image, 0, 0);
            }
            e.HasMorePages = false;
        }

        private void PrintPPT(IJobExecutionContext context)
        {
            Microsoft.Office.Interop.PowerPoint.Application PPT = new Microsoft.Office.Interop.PowerPoint.Application();//创建PPT应用
            Microsoft.Office.Interop.PowerPoint.Presentation MyPres = null;//PPT应用的实例
            //Microsoft.Office.Interop.PowerPoint.Slide MySlide = null;//PPT中的幻灯片
            var tempFilePath = context.MergedJobDataMap["tempFilePath"].ToString();
            var id = context.MergedJobDataMap["id"].ToString();
            try
            {
                //PPT.Visible = Microsoft.Office.Core.MsoTriState.msoFalse;
                MyPres = PPT.Presentations.Open(tempFilePath, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoFalse);
                MyPres.PrintOut();
                //等待文件生成
                var jobModel = PrintHandleService.GetResource().GetJobById(id).FirstOrDefault();
                if (jobModel != null)
                {
                    while (jobModel.JobStatus != "打印完成" && jobModel.JobStatus != "打印异常")
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
                if (MyPres != null)
                    MyPres.Close();
                if (PPT != null)
                    PPT.Quit();
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
            var id = context.MergedJobDataMap["id"].ToString();
            try
            {
                app = new Microsoft.Office.Interop.Word.ApplicationClass();

                app.Visible = false;
                doc = app.Documents.Open(tempFilePath);
                //打印
                object OutFileName = tempFilePath.Replace(".docx", "_print.pdf").Replace(".doc", "_print.pdf");
                //doc.PrintOut(OutputFileName: ref OutFileName);
                doc.PrintOut();
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
            var id = context.MergedJobDataMap["id"].ToString();
            try
            {
                app = new Microsoft.Office.Interop.Excel.Application(); //声明一个应用程序类实例
                                                                        //ExcelApp.DefaultFilePath = ""; //默认文件路径导出excel的路径还是在参数strFileName里设置
                                                                        //ExcelApp.DisplayAlerts = true;
                                                                        //ExcelApp.SheetsInNewWorkbook = 1;///返回或设置 Microsoft Excel 自动插入到新工作簿中的工作表数目。
                worksBook = app.Workbooks.Open(tempFilePath); //创建一个新工作簿
                Microsoft.Office.Interop.Excel.Worksheet workSheet = (Microsoft.Office.Interop.Excel.Worksheet)worksBook.Worksheets[1]; //在工作簿中得到sheet。
                object outFileName = tempFilePath.Replace(".xlsx", "_print.pdf").Replace(".xls", "_print.pdf");
                //workSheet.PrintOutEx(PrToFileName: outFileName, ActivePrinter: printName);
                workSheet.PrintOutEx();
                //等待文件生成
                var jobModel = PrintHandleService.GetResource().GetJobById(id).FirstOrDefault();
                if (jobModel != null)
                {
                    while (jobModel.JobStatus != "打印完成" && jobModel.JobStatus != "打印异常")
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
        /// <summary>
        /// 打印pdf
        /// </summary>
        /// <param name="context"></param>
        public void PrintPDF(IJobExecutionContext context)
        {
            var filePath = context.MergedJobDataMap["tempFilePath"]?.ToString();
            //PDFtoPrinter.exe打印
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            string fullFilePath = filePath;
            startInfo.FileName = GetUtilPath();
            startInfo.Arguments = " " + fullFilePath; //设定参数
            startInfo.UseShellExecute = false; //不使用系统外壳程序启动
            startInfo.RedirectStandardInput = false; //不重定向输入
            startInfo.RedirectStandardOutput = true; //重定向输出
            startInfo.CreateNoWindow = true; //不创建窗口
            //startInfo.WorkingDirectory = wrokDirectory;
            process.StartInfo = startInfo;

            try
            {
                if (process.Start()) //开始进程
                {
                    process.StandardOutput.ReadToEnd(); //读取输出流释放缓冲,  不加这一句，进程会一直无限等待
                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception :" + ex.Message);
            }
        }

        private static readonly string utilPath = GetUtilPath();

        private static Process CreateProcess(string filePath)
        {
            return new Process
            {
                StartInfo =
                    {
                    WindowStyle = ProcessWindowStyle.Hidden,
                        FileName = utilPath,
                        Arguments = $@"""{filePath}""",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
            };
        }

        private static string GetUtilPath()
        {
            return Path.Combine(
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                        "PDFtoPrinter.exe");
        }
    }

}


