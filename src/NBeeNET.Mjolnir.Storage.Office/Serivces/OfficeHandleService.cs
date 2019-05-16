using Microsoft.AspNetCore.Http;
using NBeeNET.Mjolnir.Storage.Core;
using NBeeNET.Mjolnir.Storage.Core.Common;
using NBeeNET.Mjolnir.Storage.Core.Interface;
using NBeeNET.Mjolnir.Storage.Core.Models;
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
            jsonFile.Values = OfficeInput.Jobs;
            //var task = new List<JsonFileValues>();

            ////目前仅支持在Windows
            //if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            //{
            //    //转换PDF
            //    task.Add(new JsonFileValues() { Key = "ConvertPDF", Status = "0", Value = "" });

            //    //转换PDF
            //    task.Add(new JsonFileValues() { Key = "Print", Status = "0", Value = "" });
            //}

            //jsonFile.Values = task;
            #endregion

            //保存Json
            await jsonFile.SaveAs(tempStorage.GetJsonFilePath(jsonFile.Id));

            #endregion

            //开始处理任务
            StartJob(jsonFile, tempFilePath);
            
            //删除临时目录
            //tempStorage.Delete(jsonFile.Id);
            
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

            if (jsonFile.Values?.Count > 0)
            {
                if (jsonFile.Values.Count > 0)
                {
                    for (int i = 0; i < jsonFile.Values.Count; i++)
                    {
                        JsonFileValues job = jsonFile.Values[i];
                        DebugConsole.WriteLine(jsonFile.Id + " | 正在处理任务:" + job.Key);
                        try
                        {
                            //PDF
                            if (job.Key == "CreatePDF")
                            {
                                jsonFile.Values[i] = await Task.Run(() => new Jobs.CreatePDFJob().Run(tempFilePath, job));
                            }

                            //Print
                            if (job.Key == "Print")
                            {
                                jsonFile.Values[i] = await Task.Run(() => new Jobs.PrintJob().Run(tempFilePath, job));
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                        DebugConsole.WriteLine(jsonFile.Id + " | 完成处理任务:" + job.Key);
                    }
                }
                //保存Json文件
                await jsonFile.SaveAs(tempStorage.GetJsonFilePath(jsonFile.Id));

                DebugConsole.WriteLine(jsonFile.Id + " | 结束任务处理...");
            }
            
            //复制目录
            foreach (var storageService in Register.StorageService)
            {
                await storageService.CopyDirectory(tempStorage.GetTempPath(jsonFile.Id));
            }
            DebugConsole.WriteLine(jsonFile.Id + " | 存档临时目录...");
            
            //删除临时目录
            tempStorage.Delete(jsonFile.Id);
            DebugConsole.WriteLine(jsonFile.Id + " | 删除临时目录...");
        }
        
    }
}
