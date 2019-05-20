using NBeeNET.Mjolnir.Storage.Job.Common;
using NBeeNET.Mjolnir.Storage.Job.Interface;
using System;
using System.IO;
using System.Threading.Tasks;

namespace NBeeNET.Mjolnir.Storage.Job
{
    /// <summary>
    /// 生成预览图
    /// </summary>
    public class CreateMediumJob : IJob
    {
        public string Key { get; set; } = "CreateMedium";

        public async Task Execute(IJobExecutionContext context)
        {
            var tempFilePath = context.MergedJobDataMap["tempFilePath"].ToString();

            FileInfo fileInfo = new FileInfo(tempFilePath);
            var fileName = fileInfo.Name.Replace(fileInfo.Extension, "");

            string thumbnailName = string.Format("{0}_{1}{2}", fileName, "Medium", fileInfo.Extension);
            string thumbnailPath = fileInfo.DirectoryName + "\\" + thumbnailName;

            await Task.Factory.StartNew(() => { ThumbnailClass.MakeThumbnail(tempFilePath, thumbnailPath, 200, 200, "Cut"); });

            //ThumbnailClass.MakeThumbnail(tempFilePath, thumbnailPath, 100, 100, "Cut");

            context.Result = new
            {
                Param = "200x200",
                State = "1",
                Value = thumbnailName,
                CreateTime = DateTime.Now
            };

        }
    }
}

