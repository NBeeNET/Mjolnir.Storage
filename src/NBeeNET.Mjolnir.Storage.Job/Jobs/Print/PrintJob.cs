using NBeeNET.Mjolnir.Storage.Job.Common;
using NBeeNET.Mjolnir.Storage.Job.Interface;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace NBeeNET.Mjolnir.Storage.Job
{
    /// <summary>
    /// 打印Job
    /// </summary>
    public class PrintJob : IJob
    {
        public string Key { get; set; } = "CreatePDF";

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


        private void PrintWord(IJobExecutionContext context)
        {
            Microsoft.Office.Interop.Word.Application application = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document document = null;
            var tempFilePath = context.MergedJobDataMap["tempFilePath"].ToString();
            try
            {
                FileInfo fileInfo = new FileInfo(tempFilePath);
                var fileName = fileInfo.Name.Replace(fileInfo.Extension, "");

                application.Visible = false;
                document = application.Documents.Open(tempFilePath);

                object missing = System.Reflection.Missing.Value;
                document.PrintOut(ref missing, ref missing, ref missing, ref missing,
                         ref missing, ref missing, ref missing, ref missing, ref missing,
                         ref missing, ref missing, ref missing, ref missing, ref missing,
                         ref missing, ref missing, ref missing, ref missing);

                context.Result = new
                {
                    State = "-1",
                    Value = "打印完成",
                    CreateTime = DateTime.Now
                };
            }
            catch(Exception ex)
            {
                context.Result = new
                {
                    State = "-1",
                    Value = "打印失败:"+ex.Message,
                    CreateTime = DateTime.Now
                };
            }
            finally
            {
                document.Close();
                application.Quit();
            }
        }
    }

}


