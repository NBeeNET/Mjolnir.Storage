using NBeeNET.Mjolnir.Storage.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Image.Jobs
{

    public class ConvertWebPJob
    {
        public JsonFileValues Run(string tempFilePath, JsonFileValues job)
        {

            FileInfo fileInfo = new FileInfo(tempFilePath);
            var fileName = fileInfo.Name.Replace(fileInfo.Extension, "");

            string webPName = string.Format("{0}{1}", fileName, ".webP");
            string webPPath = fileInfo.DirectoryName + "\\" + webPName;


            job.Param = "webP";
            job.Status = "1";
            job.Value = Core.StorageOperation.GetUrl(webPName);
            job.CreateTime = DateTime.Now;

           
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(tempFilePath);
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

            originalImage.Dispose();

            return job;
        }
    }
}

