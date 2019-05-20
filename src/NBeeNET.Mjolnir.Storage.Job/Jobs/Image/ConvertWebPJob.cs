using NBeeNET.Mjolnir.Storage.Job.Common;
using NBeeNET.Mjolnir.Storage.Job.Interface;
using System;
using System.IO;
using System.Threading.Tasks;

namespace NBeeNET.Mjolnir.Storage.Job
{
    /// <summary>
    /// 生成webp格式
    /// </summary>
    public class ConvertWebPJob : IJob
    {
        public string Key { get; set; } = "ConvertWebp";

        /// <summary>
        /// 执行Job
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {

            await Task.Factory.StartNew(() => { ConvertWebP(context); });
        }


        private void ConvertWebP(IJobExecutionContext context)
        {

            var tempFilePath = context.MergedJobDataMap["tempFilePath"].ToString();

            FileInfo fileInfo = new FileInfo(tempFilePath);
            var fileName = fileInfo.Name.Replace(fileInfo.Extension, "");


            string webPName = string.Format("{0}{1}", fileName, ".webP");
            string webPPath = fileInfo.DirectoryName + "\\" + webPName;

            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(tempFilePath);

            try
            {

                using (var bmp = new System.Drawing.Bitmap(originalImage.Width, originalImage.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                {
                    // 将图片重绘到新画布
                    using (var g = System.Drawing.Graphics.FromImage(bmp))
                    {
                        g.DrawImage(originalImage, 0, 0, originalImage.Width, originalImage.Height);
                    }

                    //转码并保存文件
                    using (var fs = System.IO.File.Create(webPPath))
                    {
                        Imazen.WebP.SimpleEncoder.Encode(bmp, fs, 100);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                originalImage.Dispose();
            }

            context.Result = new
            {
                Param = "webp",
                State = "1",
                Value = webPName,
                CreateTime = DateTime.Now
            };
        }
    }
}

