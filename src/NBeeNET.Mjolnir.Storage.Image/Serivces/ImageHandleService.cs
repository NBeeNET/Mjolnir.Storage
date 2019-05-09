using Microsoft.AspNetCore.Http;
using NBeeNET.Mjolnir.Storage.Core;
using NBeeNET.Mjolnir.Storage.Core.Interface;
using NBeeNET.Mjolnir.Storage.Core.Models;
using NBeeNET.Mjolnir.Storage.Image.ApiControllers.Models;
using System;
using System.Collections.Generic;
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
            //IStorageService _StorageService = new LocalStorageService();

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(DateTime.Now + ":上传图片开始...");
            //输出结果对象
            ImageOutput imageOutput = new ImageOutput();
            imageOutput.Id = Guid.NewGuid().ToString();
            imageOutput.Name = imageInput.Name;
            imageOutput.Tags = imageInput.Tags;
            imageOutput.Length = imageInput.File.Length;
            imageOutput.Type = imageInput.File.ContentType;
            imageOutput.FileName = imageOutput.Id + "." + imageInput.File.ContentType.Split("/")[1];
            imageOutput.Url = StorageOperation.GetUrl(imageOutput.FileName);
            imageOutput.Path = StorageOperation.GetPath();


            //写入临时文件夹
            var tempFilePath = await tempStorage.Write(imageInput.File, imageOutput.Id);

            if (Register._IStorageService.Count == 0)
            {
                throw new Exception("必须添加存储服务");
            }

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(DateTime.Now + ":开始复制目录...");
            //复制目录
            foreach (var storageService in Register._IStorageService)
            {
                await storageService.CopyDirectory(tempStorage.GetTempPath(imageOutput.Id));
            }


            //保存Json文件
            JsonFile jsonFile = new JsonFile();
            jsonFile.Id = imageOutput.Id;
            jsonFile.CreateTime = DateTime.Now;
            jsonFile.Name = imageOutput.Name;
            jsonFile.Tags = imageOutput.Tags;
            jsonFile.Url = imageOutput.Url;
            jsonFile.FileName = imageOutput.FileName;

            //创建处理作业
            var task = new List<JsonFileValues>();
            //预览图
            task.Add(new JsonFileValues() { Key = "Medium", Status = "0", Value = "" });
            //缩略图
            task.Add(new JsonFileValues() { Key = "Small", Status = "0", Value = "" });
            //WebP格式
            task.Add(new JsonFileValues() { Key = "WebP", Status = "0", Value = "" });

            jsonFile.Values = task;
            await jsonFile.SaveAs(tempStorage.GetJsonFilePath(jsonFile.Id));

            //开始处理任务
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(DateTime.Now + ":任务处理开始...");
            StartJob(jsonFile, tempFilePath);

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(DateTime.Now + ":上传图片结束...");
            //返回结果
            return imageOutput;
        }

        /// <summary>
        /// 多图片上传处理
        /// </summary>
        /// <param name="imageInput"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<ImageOutput>> ProcessingImages(List<ImageInput> imageInput, HttpRequest request)
        {
            List<ImageOutput> output = new List<ImageOutput>();
            for (int i = 0; i < imageInput.Count; i++)
            {
                var result = await Processing(imageInput[i], request);
                output.Add(result);
            }
            return output;
        }

        /// <summary>
        /// 开始处理任务
        /// </summary>
        /// <returns></returns>
        public async Task StartJob(JsonFile jsonFile, string tempFilePath)
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
                        Console.WriteLine("正在处理图片:" + job.Key);
                        try
                        {
                            //预览图处理
                            if (job.Key == "Medium")
                            {
                                jsonFile.Values.Add(new Jobs.CreateMediumJob().Run(tempFilePath, job));
                            }
                            //缩略图处理
                            if (job.Key == "Small")
                            {
                                jsonFile.Values.Add(new Jobs.CreateSmallJob().Run(tempFilePath, job));
                            }
                            //WebP格式转换
                            //if (job.Key == "WebP")
                            //{
                            //    jsonFile.Values.Add(new Jobs.ConvertWebPJob().Run(tempFilePath, job));
                            //}
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                        Console.WriteLine("处理图片结束:" + job.Key);
                    }
                }
                //保存Json文件
                await jsonFile.SaveAs(tempStorage.GetJsonFilePath(jsonFile.Id));

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(DateTime.Now + ":任务处理完成...");

                Console.WriteLine(DateTime.Now + ":再次复制目录...");
                //复制目录
                foreach (var storageService in Register._IStorageService)
                {
                    await storageService.CopyDirectory(tempStorage.GetTempPath(jsonFile.Id));
                }

                //删除临时目录
                await tempStorage.Delete(jsonFile.Id);

            }
        }
    }
}
