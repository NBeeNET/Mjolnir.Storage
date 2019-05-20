using NBeeNET.Mjolnir.Storage.Job.Common;
using NBeeNET.Mjolnir.Storage.Job.Interface;
using NBeeNET.Mjolnir.Storage.Print.Common;
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
        public string Key { get; set; } = "Print";

        /// <summary>
        /// Job执行函数
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
            var tempFilePath = context.MergedJobDataMap["tempFilePath"].ToString();

            if (!File.Exists(tempFilePath))
            {
                context.Result = new
                {
                    State = "-1",
                    Value = "打印失败：文件不存在",
                    CreateTime = DateTime.Now
                };

                return;
            }

            FileInfo fileInfo = new FileInfo(tempFilePath);
            var fileName = fileInfo.Name.Replace(fileInfo.Extension, "");

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                switch (fileInfo.Extension.ToUpper())
                {
                    case ".XLS":
                    case ".XLSX":

                        PrintHelper.ExcelPrintAsync(tempFilePath);
                        break;
                    case ".DOC":
                    case ".DOCX":
                    case ".TXT":
                        PrintHelper.WordPrintAsync(tempFilePath);
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
            catch (Exception ex)
            {

                context.Result = new
                {
                    State = "-1",
                    Value = "打印失败："+ex.Message,
                    CreateTime = DateTime.Now
                };
            }

          
        }

    }

}


