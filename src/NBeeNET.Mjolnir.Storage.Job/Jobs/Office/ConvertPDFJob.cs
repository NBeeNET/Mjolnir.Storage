using NBeeNET.Mjolnir.Storage.Job.Common;
using NBeeNET.Mjolnir.Storage.Job.Interface;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace NBeeNET.Mjolnir.Storage.Job
{
    /// <summary>
    /// 转换Pdf格式
    /// </summary>
    public class ConvertPDFJob : IJob
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
                            WordToPDF(context);
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

        private void WordToPDF(IJobExecutionContext context)
        {
            var tempFilePath = context.MergedJobDataMap["tempFilePath"].ToString();
            Microsoft.Office.Interop.Word.Application application = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document document = null;

            try
            {
                FileInfo fileInfo = new FileInfo(tempFilePath);
                var fileName = fileInfo.Name.Replace(fileInfo.Extension, "");
                string pdfName = string.Format("{0}{1}", fileName, ".pdf");
                string pdfPath = fileInfo.DirectoryName + "\\" + pdfName;

                application.Visible = false;
                document = application.Documents.Open(tempFilePath);
                if (!File.Exists(pdfPath))
                    document.ExportAsFixedFormat(pdfPath, Microsoft.Office.Interop.Word.WdExportFormat.wdExportFormatPDF);

                context.Result = new
                {
                    State = "1",
                    Value = pdfName,
                    CreateTime = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                context.Result = new
                {
                    State = "-1",
                    Value = "转换失败：" + ex.Message,
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
