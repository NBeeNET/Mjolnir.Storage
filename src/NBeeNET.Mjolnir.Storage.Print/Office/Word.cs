using NBeeNET.Mjolnir.Storage.Print.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NBeeNET.Mjolnir.Storage.Print.Office
{
    public class Word
    {
        /// <summary>
        /// 同步打印
        /// </summary>
        /// <param name="path"></param>
        /// <param name="msg"></param>
        /// <param name="printerName">打印机名称</param>
        public static void Print(string path, out string msg,object outFileName = null, string printerName = null)
        {
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
                if(string.IsNullOrEmpty(outFileName?.ToString()))
                    outFileName = path.ToLower().Replace(".docx", ".pdf").Replace(".doc", ".pdf");
                doc.PrintOut(OutputFileName: ref outFileName);
                msg = "打印完成";
                //doc.PrintOut(ref missing, ref missing, ref missing, ref missing,
                //     ref missing, ref missing, ref missing, ref missing, ref missing,
                //     ref missing, ref missing, ref missing, ref missing, ref missing,
                //     ref missing, ref missing, ref missing, ref missing);
            }
            catch (Exception exp)
            {
                msg = "打印异常";
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
        }
        /// <summary>
        /// 异步打印
        /// </summary>
        /// <param name="path"></param>
        /// <param name="jobModel"></param>
        public static async void PrintAsync(string path, object outFileName = null, string printerName = null, PrintJobModel jobModel = null)
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
