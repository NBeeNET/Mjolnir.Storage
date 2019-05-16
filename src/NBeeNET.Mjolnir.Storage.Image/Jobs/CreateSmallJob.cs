using NBeeNET.Mjolnir.Storage.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Image.Jobs
{
    /// <summary>
    /// 生成缩略图
    /// </summary>
    public class CreateSmallJob
    {
        public JsonFileDetail Run(string tempFilePath, JsonFileDetail job)
        {
            FileInfo fileInfo = new FileInfo(tempFilePath);
            var fileName = fileInfo.Name.Replace(fileInfo.Extension, "");

            string thumbnailName = string.Format("{0}_{1}{2}", fileName, "Small", fileInfo.Extension);
            string thumbnailPath = fileInfo.DirectoryName + "\\" + thumbnailName;

            ThumbnailClass.MakeThumbnail(tempFilePath, thumbnailPath, 100, 100, "Cut");
            job.Param = "100x100";
            job.State = "1";
            job.Value = Core.StorageOperation.GetUrl(thumbnailName);
            job.CreateTime = DateTime.Now;

            return job;
        }
    }
}
