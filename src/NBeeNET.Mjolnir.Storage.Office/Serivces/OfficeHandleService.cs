﻿using Microsoft.AspNetCore.Http;
using NBeeNET.Mjolnir.Storage.Core;
using NBeeNET.Mjolnir.Storage.Core.Common;
using NBeeNET.Mjolnir.Storage.Core.Interface;
using NBeeNET.Mjolnir.Storage.Core.Models;
using NBeeNET.Mjolnir.Storage.Job;
using NBeeNET.Mjolnir.Storage.Job.Implement;
using NBeeNET.Mjolnir.Storage.Office.ApiControllers.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace NBeeNET.Mjolnir.Storage.Office.Serivces
{
    /// <summary>
    /// Office处理
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
        public async Task<OfficeOutput> Save(OfficeInput OfficeInput)
        {
            if (Register.StorageService.Count == 0)
            {
                throw new Exception("必须添加存储服务");
            }

            TempStorageOperation tempStorage = new TempStorageOperation();

            //输出结果对象
            OfficeOutput OfficeOutput = new OfficeOutput();
            OfficeOutput.Id = Guid.NewGuid().ToString();
            OfficeOutput.Type = OfficeInput.File.FileName.Split('.')[OfficeInput.File.FileName.Split('.').Length - 1];
            OfficeOutput.FileName = OfficeOutput.Id + "." + OfficeOutput.Type;
            OfficeOutput.Name = OfficeInput.Name;
            OfficeOutput.Tags = OfficeInput.Tags;
            OfficeOutput.Length = OfficeInput.File.Length;
            OfficeOutput.Url = StorageOperation.GetUrl(OfficeOutput.FileName);
            OfficeOutput.FilePath = StorageOperation.GetUrl(OfficeOutput.FileName);
            OfficeOutput.Path = StorageOperation.GetPath();

            //写入临时文件夹
            var tempFilePath = await tempStorage.Write(OfficeInput.File, OfficeOutput.Id);
            
            //复制目录
            foreach (var storageService in Register.StorageService)
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

            #region 创建处理作业
            jsonFile.Details = OfficeInput.Jobs;
            #endregion

            //保存Json
            await jsonFile.SaveAs(tempStorage.GetJsonFilePath(jsonFile.Id));

            #endregion

            //开始处理任务
            StartJob(jsonFile, tempFilePath);
            
            
            return OfficeOutput;
        }

        /// <summary>
        /// 多文件保存
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public async Task<List<OfficeOutput>> MultiSave(List<OfficeInput> inputs)
        {
            List<OfficeOutput> output = new List<OfficeOutput>();
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
                Scheduler scheduler = new Scheduler("Test");

                for (int i = 0; i < jsonFile.Details.Count; i++)
                {
                    var jobContext = JobBuilder.Create<PrintJob>()
                         .WithName("print")
                         .AddJobData("tempFilePath", tempFilePath)
                         .Initialize();
                    scheduler.AddJob(jobContext);
                }
                await scheduler.Start();
            }
            
            //复制目录
            foreach (var storageService in Register.StorageService)
            {
                await storageService.CopyDirectory(tempStorage.GetTempPath(jsonFile.Id));
            }
            DebugConsole.WriteLine(jsonFile.Id + " | 存档临时目录...");

            //删除临时目录
            if (isDeleteTempDirectory)
            {
                tempStorage.Delete(jsonFile.Id);
                DebugConsole.WriteLine(jsonFile.Id + " | 删除临时目录...");
            }
        }
        
    }
}
