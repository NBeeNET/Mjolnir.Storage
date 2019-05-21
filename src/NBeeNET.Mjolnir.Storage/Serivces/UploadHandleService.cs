using Microsoft.AspNetCore.Http;
using NBeeNET.Mjolnir.Storage.Models;
using NBeeNET.Mjolnir.Storage.Core;
using NBeeNET.Mjolnir.Storage.Core.Interface;
using NBeeNET.Mjolnir.Storage.Core.Models;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace NBeeNET.Mjolnir.Storage
{
    /// <summary>
    /// 上传处理
    /// </summary>
    public class UploadHandleService
    {

        public UploadHandleService()
        {

        }

        /// <summary>
        /// 单文件上传处理
        /// </summary>
        /// <param name="imageInput"></param>
        /// <returns></returns>
        public async Task<UploadOutput> Save(UploadInput imageInput, HttpRequest request)
        {
            TempStorageOperation tempStorage = new TempStorageOperation();
            //IStorageService _StorageService = new LocalStorageService();

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(DateTime.Now + ":上传图片开始...");
            //输出结果对象
            UploadOutput imageOutput = new UploadOutput();
            imageOutput.Id = Guid.NewGuid().ToString();
            imageOutput.Name = imageInput.Name;
            imageOutput.Tags = imageInput.Tags;
            imageOutput.Length = imageInput.File.Length;
            imageOutput.Type = imageInput.File.FileName.Split('.')[imageInput.File.FileName.Split('.').Length - 1];
            imageOutput.FileName = imageOutput.Id + "." + imageOutput.Type;
            imageOutput.Url = StorageOperation.GetUrl(imageOutput.FileName);
            imageOutput.Path = StorageOperation.GetPath();


            //写入临时文件夹
            var tempFilePath = await tempStorage.Write(imageInput.File, imageOutput.Id);

            if (Register.StorageService.Count == 0)
            {
                throw new Exception("必须添加存储服务");
            }

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(DateTime.Now + ":开始复制目录...");
            //复制目录
            foreach (var storageService in Register.StorageService)
            {
                await storageService.CopyDirectory(tempStorage.GetTempPath(imageOutput.Id));
            }

            #region 生成Json
            //保存Json文件
            JsonFile jsonFile = new JsonFile();
            jsonFile.Id = imageOutput.Id;
            jsonFile.CreateTime = DateTime.Now;
            jsonFile.Name = imageOutput.Name;
            jsonFile.Tags = imageOutput.Tags;
            jsonFile.Url = imageOutput.Url;
            jsonFile.FileName = imageOutput.FileName;

            //创建处理作业
            var task = new List<JsonFileDetail>();
            //预览图
            task.Add(new JsonFileDetail() { Key = "Medium", State = "0", Value = "" });
            //缩略图
            task.Add(new JsonFileDetail() { Key = "Small", State = "0", Value = "" });

            //.Net Core生成 WebP格式,目前仅支持在Windows
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                //WebP格式
                task.Add(new JsonFileDetail() { Key = "WebP", State = "0", Value = "" });
            }

            jsonFile.Details = task;
            await jsonFile.SaveAs(tempStorage.GetJsonFilePath(jsonFile.Id));

            #endregion

            //开始处理任务
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(DateTime.Now + ":任务处理开始...");
            StartJob(jsonFile, tempFilePath);

            //删除临时目录
            //tempStorage.Delete(jsonFile.Id);

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(DateTime.Now + ":上传结束...");
            //返回结果

            return imageOutput;
        }

        /// <summary>
        /// 多文件上传处理
        /// </summary>
        /// <param name="imageInput"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<UploadOutput>> MultiSave(List<UploadInput> imageInput, HttpRequest request)
        {
            List<UploadOutput> output = new List<UploadOutput>();
            for (int i = 0; i < imageInput.Count; i++)
            {
                var result = await Save(imageInput[i], request);
                output.Add(result);
            }
            return output;
        }

        /// <summary>
        /// 开始处理任务
        /// </summary>
        /// <returns></returns>
        public async void StartJob(JsonFile jsonFile, string tempFilePath)
        {
            StorageOperation storage = new StorageOperation();
            TempStorageOperation tempStorage = new TempStorageOperation();
            if (jsonFile.Details?.Count > 0)
            {
                
                Queue<JsonFileDetail> queues = new Queue<JsonFileDetail>();
                for (int i = 0; i < jsonFile.Details.Count; i++)
                {
                    queues.Enqueue(jsonFile.Details[i]);
                }

                jsonFile.Details.Clear();
                if (queues.Count > 0)
                {
                    JsonFileDetail job = null;
                    //while (queues.TryDequeue(out job))
                    //{
                    //    Console.WriteLine("正在处理图片:" + job.Key);
                    //    try
                    //    {
                    //        ////预览图处理
                    //        //if (job.Key == "Medium")
                    //        //{
                    //        //    jsonFile.Details.Add(new Jobs.CreateMediumJob().Run(tempFilePath, job));
                    //        //}
                    //        ////缩略图处理
                    //        //if (job.Key == "Small")
                    //        //{
                    //        //    jsonFile.Details.Add(new Jobs.CreateSmallJob().Run(tempFilePath, job));
                    //        //}
                    //        ////WebP格式转换
                    //        //if (job.Key == "WebP")
                    //        //{
                    //        //    jsonFile.Details.Add(new Jobs.ConvertWebPJob().Run(tempFilePath, job));
                    //        //}
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Console.WriteLine(ex.ToString());
                    //    }
                    //    Console.WriteLine("处理图片结束:" + job.Key);
                    //}
                }
                //保存Json文件
                await jsonFile.SaveAs(tempStorage.GetJsonFilePath(jsonFile.Id));

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(DateTime.Now + ":任务处理完成...");

                Console.WriteLine(DateTime.Now + ":再次复制目录...");            

            }
            //复制目录
            foreach (var storageService in Register.StorageService)
            {
                await storageService.CopyDirectory(tempStorage.GetTempPath(jsonFile.Id));
            }
            //删除临时目录
            tempStorage.Delete(jsonFile.Id);
        }
    }
}
