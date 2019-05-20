using NBeeNET.Mjolnir.Storage.Job.Common;
using NBeeNET.Mjolnir.Storage.Job.Interface;
using System;
using System.IO;
using System.Threading.Tasks;

namespace NBeeNET.Mjolnir.Storage.Job
{
    /// <summary>
    /// 生成缩略图
    /// </summary>
    public class CreateSmallJob : IJob
    {
        public string Key { get; set; } = "CreateSmall";


        public async Task Execute(IJobExecutionContext context)
        {
            var tempFilePath = context.MergedJobDataMap["tempFilePath"].ToString();

            FileInfo fileInfo = new FileInfo(tempFilePath);
            var fileName = fileInfo.Name.Replace(fileInfo.Extension, "");

            string thumbnailName = string.Format("{0}_{1}{2}", fileName, "Small", fileInfo.Extension);
            string thumbnailPath = fileInfo.DirectoryName + "\\" + thumbnailName;

            await Task.Factory.StartNew(() => { ThumbnailClass.MakeThumbnail(tempFilePath, thumbnailPath, 100, 100, "Cut"); });

            //ThumbnailClass.MakeThumbnail(tempFilePath, thumbnailPath, 100, 100, "Cut");

            context.Result = new
            {
                Param = "100x100",
                State = "1",
                Value = thumbnailName,
                CreateTime = DateTime.Now
            };

        }
    }
}
