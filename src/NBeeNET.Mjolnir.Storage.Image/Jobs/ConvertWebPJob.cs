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

            string webPName = string.Format("{0}_{1}", fileName, ".webP");
            string webPPath = fileInfo.DirectoryName + "\\" + webPName;


            job.Param = "webP";
            job.Status = "1";
            job.Value = Core.StorageOperation.GetUrl(webPName);
            job.CreateTime = DateTime.Now;

            Imazen.WebP.SimpleEncoder simpleEncoder = new Imazen.WebP.SimpleEncoder();
            System.Drawing.Image image = System.Drawing.Image.FromFile(tempFilePath);
            using (var bmp = new System.Drawing.Bitmap(image.Width, image.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
            {
                // 将图片重绘到新画布
                using (var g = System.Drawing.Graphics.FromImage(bmp))
                {
                    g.DrawImage(image, 0, 0, image.Width, image.Height);
                }

                //转码并保存文件
                using (var fs = System.IO.File.Create(webPPath))
                {
                    simpleEncoder.Encode(bmp, fs, 100);
                }
            }

            return job;
        }
    }
}

