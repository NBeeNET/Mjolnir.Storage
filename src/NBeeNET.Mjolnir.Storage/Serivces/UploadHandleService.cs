using Microsoft.AspNetCore.Http;
using NBeeNET.Mjolnir.Storage.Core;
using NBeeNET.Mjolnir.Storage.Core.Common;
using NBeeNET.Mjolnir.Storage.Core.Implement;
using NBeeNET.Mjolnir.Storage.Core.Interface;
using NBeeNET.Mjolnir.Storage.Core.Models;
using NBeeNET.Mjolnir.Storage.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace NBeeNET.Mjolnir.Storage.Serivces
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
        /// 保存Upload文件
        /// </summary>
        /// <param name="UploadInput"></param>
        /// <returns></returns>
        public async Task<UploadOutput> Save(UploadInput UploadInput)
        {
            if (Register.StorageService.Count == 0)
            {
                throw new Exception("必须添加存储服务");
            }

            TempStorageOperation tempStorage = new TempStorageOperation();

            //输出结果对象
            UploadOutput UploadOutput = new UploadOutput();
            UploadOutput.Id = Guid.NewGuid().ToString();
            UploadOutput.Type = UploadInput.File.FileName.Split('.')[UploadInput.File.FileName.Split('.').Length - 1];
            UploadOutput.FileName = UploadOutput.Id + "." + UploadOutput.Type;
            UploadOutput.Name = UploadInput.Name;
            UploadOutput.Tags = UploadInput.Tags;
            UploadOutput.Length = UploadInput.File.Length;
            UploadOutput.Url = StorageOperation.GetUrl(UploadOutput.FileName);
            UploadOutput.FilePath = StorageOperation.GetUrl(UploadOutput.FileName);
            UploadOutput.Path = StorageOperation.GetPath();

            //写入临时文件夹
            var tempFilePath = await tempStorage.Write(UploadInput.File, UploadOutput.Id);

            //复制目录
            foreach (var storageService in Register.StorageService)
            {
                await storageService.CopyDirectory(tempStorage.GetTempPath(UploadOutput.Id));
            }

            #region 生成Json
            //保存Json文件
            JsonFile jsonFile = new JsonFile();
            jsonFile.Id = UploadOutput.Id;
            jsonFile.CreateTime = DateTime.Now;
            jsonFile.Name = UploadOutput.Name;
            jsonFile.Tags = UploadOutput.Tags;
            jsonFile.Url = UploadOutput.Url;
            jsonFile.FileName = UploadOutput.FileName;

            #region 创建处理作业
            jsonFile.Details = UploadInput.Jobs;
            #endregion

            //保存Json
            await jsonFile.SaveAs(tempStorage.GetJsonFilePath(jsonFile.Id));

            #endregion

            //开始处理任务
            StartJob(jsonFile, tempFilePath);


            return UploadOutput;
        }

        /// <summary>
        /// 多文件保存
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public async Task<List<UploadOutput>> MultiSave(List<UploadInput> inputs)
        {
            List<UploadOutput> output = new List<UploadOutput>();
            for (int i = 0; i < inputs.Count; i++)
            {
                var result = await Save(inputs[i]);
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

            DebugConsole.WriteLine(jsonFile.Id + " | 开始处理任务...");
            bool isDeleteTempDirectory = true;

            if (jsonFile.Details?.Count > 0)
            {
                Scheduler scheduler = new Scheduler("UploadHandleService");
                for (int i = 0; i < jsonFile.Details.Count; i++)
                {
                    JsonFileDetail job = jsonFile.Details[i];
                    DebugConsole.WriteLine(jsonFile.Id + " | 正在处理任务:" + job.Key);
                    try
                    {

                        if (job.Key.Contains("Client"))
                        {
                            isDeleteTempDirectory = false;
                            continue;
                        }

                        //判断Job类型是否有
                        IJob outJob = null;
                        var ret = Register.TryGetJob(job.Key, out outJob);
                        if (ret)
                        {
                            //添加Job

                            var jobContext = JobBuilder.Create(outJob.GetType())
                           .WithName(job.Key)
                           .AddJobData("tempFilePath", tempFilePath)
                           .Initialize();
                            scheduler.AddJob(jobContext);

                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    DebugConsole.WriteLine(jsonFile.Id + " | 完成处理任务:" + job.Key);
                }
                //开始处理任务
                await scheduler.Start();
            }
            //保存Json文件
            await jsonFile.SaveAs(tempStorage.GetJsonFilePath(jsonFile.Id));

            DebugConsole.WriteLine(jsonFile.Id + " | 结束任务处理...");


            //复制目录
            foreach (var storageService in Register.StorageService)
            {
                await storageService.CopyDirectory(tempStorage.GetTempPath(jsonFile.Id));
            }
            DebugConsole.WriteLine(jsonFile.Id + " | 存档临时目录...");

            //删除临时目录
            //if (isDeleteTempDirectory)
            //{
            //    //tempStorage.Delete(jsonFile.Id);
            //    //DebugConsole.WriteLine(jsonFile.Id + " | 删除临时目录...");
            //}
        }

    }
}
