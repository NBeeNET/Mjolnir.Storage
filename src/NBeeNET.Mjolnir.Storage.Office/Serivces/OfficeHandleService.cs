using Microsoft.AspNetCore.Http;
using NBeeNET.Mjolnir.Storage.Core;
using NBeeNET.Mjolnir.Storage.Core.Interface;
using NBeeNET.Mjolnir.Storage.Core.Models;
using NBeeNET.Mjolnir.Storage.Office.ApiControllers.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace NBeeNET.Mjolnir.Storage.Office.Serivces
{
    /// <summary>
    /// 图片处理
    /// </summary>
    public class OfficeHandleService
    {

        public OfficeHandleService()
        {
          
        }

        /// <summary>
        /// 保存office文件
        /// </summary>
        /// <param name="OfficeInput"></param>
        /// <returns></returns>
        public async Task<OfficeOutput> Save(OfficeInput OfficeInput, HttpRequest request)
        {
            TempStorageOperation tempStorage = new TempStorageOperation();
            
            //输出结果对象
            OfficeOutput OfficeOutput = new OfficeOutput();

            OfficeOutput.Id = Guid.NewGuid().ToString();
            OfficeOutput.Name = OfficeInput.Name;
            OfficeOutput.Tags = OfficeInput.Tags;
            OfficeOutput.Length = OfficeInput.File.Length;
            OfficeOutput.Type = OfficeInput.File.FileName.Split('.')[OfficeInput.File.FileName.Split('.').Length - 1];
            OfficeOutput.FileName = OfficeOutput.Id + "." + OfficeOutput.Type;
            OfficeOutput.Url = StorageOperation.GetUrl(OfficeOutput.FileName);
            OfficeOutput.Path = StorageOperation.GetPath();


            //写入临时文件夹
            var tempFilePath = await tempStorage.Write(OfficeInput.File, OfficeOutput.Id);

            if (Register._IStorageService.Count == 0)
            {
                throw new Exception("必须添加存储服务");
            }

            //复制目录
            foreach (var storageService in Register._IStorageService)
            {
                await storageService.CopyDirectory(tempStorage.GetTempPath(OfficeOutput.Id));
            }

            #region 生成Json
            //保存Json文件
            JsonFile jsonFile = new JsonFile();
            jsonFile.Id = OfficeOutput.Id;
            jsonFile.CreateTime = DateTime.Now;
            jsonFile.Name = OfficeOutput.Name;
            jsonFile.Tags = OfficeOutput.Tags;
            jsonFile.Url = OfficeOutput.Url;
            jsonFile.FileName = OfficeOutput.FileName;

            await jsonFile.SaveAs(tempStorage.GetJsonFilePath(jsonFile.Id));

            #endregion

            //开始处理任务
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(DateTime.Now + ":任务处理开始...");
            StartJob(jsonFile, tempFilePath);

            //删除临时目录
            tempStorage.Delete(jsonFile.Id);

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(DateTime.Now + ":上传结束...");
            //返回结果

            return OfficeOutput;
        }

        /// <summary>
        /// 多文件保存
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<OfficeOutput>> MultiSave(List<OfficeInput> inputs, HttpRequest request)
        {
            List<OfficeOutput> output = new List<OfficeOutput>();
            for (int i = 0; i < inputs.Count; i++)
            {
                var result = await Save(inputs[i], request);
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

            if (jsonFile.Values?.Count > 0)
            {
               
                Queue<JsonFileValues> queues = new Queue<JsonFileValues>();
                for (int i = 0; i < jsonFile.Values.Count; i++)
                {
                    queues.Enqueue(jsonFile.Values[i]);
                }

                jsonFile.Values.Clear();
                if (queues.Count > 0)
                {
                    JsonFileValues job = null;
                    //while (queues.TryDequeue(out job))
                    //{
                    //    Console.WriteLine("正在处理图片:" + job.Key);
                    //    try
                    //    {
                    //        //预览图处理
                    //        if (job.Key == "Medium")
                    //        {
                    //            jsonFile.Values.Add(new Jobs.CreateMediumJob().Run(tempFilePath, job));
                    //        }
                    //        //缩略图处理
                    //        if (job.Key == "Small")
                    //        {
                    //            jsonFile.Values.Add(new Jobs.CreateSmallJob().Run(tempFilePath, job));
                    //        }
                    //        //WebP格式转换
                    //        if (job.Key == "WebP")
                    //        {
                    //            jsonFile.Values.Add(new Jobs.ConvertWebPJob().Run(tempFilePath, job));
                    //        }
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
            foreach (var storageService in Register._IStorageService)
            {
                await storageService.CopyDirectory(tempStorage.GetTempPath(jsonFile.Id));
            }
        }


    }
}
