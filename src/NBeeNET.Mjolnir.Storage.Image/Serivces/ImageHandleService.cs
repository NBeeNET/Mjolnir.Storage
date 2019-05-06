using Microsoft.AspNetCore.Http;
using NBeeNET.Mjolnir.Storage.Core;
using NBeeNET.Mjolnir.Storage.Core.Interface;
using NBeeNET.Mjolnir.Storage.Core.Models;
using NBeeNET.Mjolnir.Storage.Image.ApiControllers.Models;
using NBeeNET.Mjolnir.Storage.Local.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace NBeeNET.Mjolnir.Storage.Image.Serivces
{
    /// <summary>
    /// 图片处理
    /// </summary>
    public class ImageHandleService
    {

        public ImageHandleService()
        {

        }

        /// <summary>
        /// 处理图片
        /// </summary>
        /// <param name="imageInput"></param>
        /// <returns></returns>
        public async Task<ImageOutput> Processing(ImageInput imageInput, HttpRequest request)
        {
            TempStorageOperation tempStorage = new TempStorageOperation();
            StorageOperation storage = new StorageOperation();
            IStorageService _StorageService = new LocalStorageService();

            //输出结果对象
            ImageOutput imageOutput = new ImageOutput();
            string id = Guid.NewGuid().ToString();
            imageOutput.Id = id;
            imageOutput.Name = imageInput.Name;
            imageOutput.Tags = imageInput.Tags;
            imageOutput.Length = imageInput.File.Length;
            imageOutput.Type = imageInput.File.ContentType;
            string fileName = id + "." + imageInput.File.ContentType.Split("/")[1];
            string path = storage.GetSavePath(id) + "\\" + fileName;
            string url = new StringBuilder()
                           .Append(request.Scheme)
                           .Append("://")
                           .Append(request.Host)
                           .Append(path.Split("wwwroot")[1])
                           .ToString().Replace("\\", "/");

            imageOutput.Url = url;
            imageOutput.Path = path.Split("wwwroot")[1];

            //写入临时文件夹
            var tempPath = await tempStorage.Write(imageInput.File, id);

            //复制目录
            await _StorageService.CopyDirectory(tempStorage.GetTempPath(id), storage.GetSavePath(id), true);

            //保存Json文件
            JsonFile jsonFile = new JsonFile();
            jsonFile.Id = id;
            jsonFile.CreateTime = DateTime.Now;
            jsonFile.Name = imageInput.Name;
            jsonFile.Tags = imageInput.Tags;
            jsonFile.PathUrl = tempPath;
            var task = new List<JsonFileValues>();
            task.Add(new JsonFileValues() { Key = "MakeThumbnail", Param = "{}", Status = "0", Value = "" });
            jsonFile.Values = task;
            await jsonFile.SaveAs(tempStorage.GetJsonFilePath(jsonFile.Id));

            //开始处理任务
            await StartJob(jsonFile);

            //复制目录
            await _StorageService.CopyDirectory(tempStorage.GetTempPath(jsonFile.Id), storage.GetSavePath(jsonFile.Id), true);

            //删除临时目录
            await tempStorage.Delete(jsonFile.Id);

            //返回结果
            return imageOutput;
        }



        /// <summary>
        /// 开始处理任务
        /// </summary>
        /// <returns></returns>
        public async Task StartJob(JsonFile jsonFile)
        {
            if (jsonFile.Values.Count > 0)
            {
                StorageOperation storage = new StorageOperation();
                TempStorageOperation tempStorage = new TempStorageOperation();
                Queue<JsonFileValues> queues = new Queue<JsonFileValues>();
                for (int i = 0; i < jsonFile.Values.Count; i++)
                {
                    queues.Enqueue(jsonFile.Values[i]);
                }

                jsonFile.Values.Clear();
                if (queues.Count > 0)
                {
                    JsonFileValues job = null;
                    while (queues.TryDequeue(out job))
                    {
                        if (job.Key == "MakeThumbnail")
                        {
                            FileInfo fileInfo = new FileInfo(jsonFile.PathUrl);
                            var fileName = fileInfo.Name.Replace(fileInfo.Extension, "");
                            string thumbnailUrl = string.Format("{0}\\{1}_{2}{3}", fileInfo.DirectoryName, fileName, "100X100", fileInfo.Extension);
                            ThumbnailClass.MakeThumbnail(jsonFile.PathUrl, thumbnailUrl, 100, 100, "Cut");
                            job.Status = "1";
                            job.Value = thumbnailUrl;
                            jsonFile.Values.Add(job);
                        }
                    }
                }
                //保存Json文件
                await jsonFile.SaveAs(tempStorage.GetJsonFilePath(jsonFile.Id));


            }
        }
    }
}
