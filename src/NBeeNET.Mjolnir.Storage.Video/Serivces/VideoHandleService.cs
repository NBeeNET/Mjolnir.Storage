using Microsoft.AspNetCore.Http;
using NBeeNET.Mjolnir.Storage.Video.ApiControllers.Models;
using NBeeNET.Mjolnir.Storage.Core;
using NBeeNET.Mjolnir.Storage.Core.Interface;
using NBeeNET.Mjolnir.Storage.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NBeeNET.Mjolnir.Storage.Video.Serivces
{
    /// <summary>
    /// 视频处理
    /// </summary>
    public class VideoHandleService
    {

        public VideoHandleService()
        {

        }

        /// <summary>
        /// 处理视频
        /// </summary>
        /// <param name="imageInput"></param>
        /// <returns></returns>
        public async Task<VideoOutput> Processing(VideoInput input, HttpRequest request)
        {
            TempStorageOperation tempStorage = new TempStorageOperation();
            //IStorageService _StorageService = new LocalStorageService();

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(DateTime.Now + ":上传视频开始...");
            //输出结果对象
            VideoOutput output = new VideoOutput();
            output.Id = Guid.NewGuid().ToString();
            output.Name = input.Name;
            output.Tags = input.Tags;
            output.Length = input.File.Length;
            output.Type = input.File.ContentType;
            output.FileName = output.Id + "." + input.File.ContentType.Split("/")[1];
            output.Url = StorageOperation.GetUrl(output.FileName);
            output.Path = StorageOperation.GetPath();


            //写入临时文件夹
            var tempFilePath = await tempStorage.Write(input.File, output.Id);

            if (Register._IStorageService.Count == 0)
            {
                throw new Exception("必须添加存储服务");
            }

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(DateTime.Now + ":开始复制目录...");
            //复制目录
            foreach (var storageService in Register._IStorageService)
            {
                await storageService.CopyDirectory(tempStorage.GetTempPath(output.Id));
            }


            //保存Json文件
            JsonFile jsonFile = new JsonFile();
            jsonFile.Id = output.Id;
            jsonFile.CreateTime = DateTime.Now;
            jsonFile.Name = output.Name;
            jsonFile.Tags = output.Tags;
            jsonFile.Url = output.Url;
            jsonFile.FileName = output.FileName;
            await jsonFile.SaveAs(tempStorage.GetJsonFilePath(jsonFile.Id));

          

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(DateTime.Now + ":上传视频结束...");
            //返回结果
            return output;
        }


        /// <summary>
        /// 多视频上传处理
        /// </summary>
        /// <param name="imageInput"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<VideoOutput>> ProcessingImages(List<VideoInput> inputs, HttpRequest request)
        {
            List<VideoOutput> output = new List<VideoOutput>();
            for (int i = 0; i < inputs.Count; i++)
            {
                var result = await Processing(inputs[i], request);
                output.Add(result);
            }
            return output;
        }
    }
}
