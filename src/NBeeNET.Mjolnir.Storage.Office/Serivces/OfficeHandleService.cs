using Microsoft.AspNetCore.Http;
using NBeeNET.Mjolnir.Storage.Core;
using NBeeNET.Mjolnir.Storage.Core.Interface;
using NBeeNET.Mjolnir.Storage.Core.Models;
using NBeeNET.Mjolnir.Storage.Office.Jobs;
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
            jobList.Add(new PrintJob());
        }
        /// <summary>
        /// 执行job列表
        /// </summary>
        private List<IJob> jobList = new List<IJob>();

        /// <summary>
        /// 执行保存和删除临时文件
        /// </summary>
        /// <param name="OfficeInput"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<OfficeOutput> SaveAndDelete(OfficeInput OfficeInput, HttpRequest request)
        {
            TempStorageOperation tempStorage = new TempStorageOperation();
            OfficeOutput officeOutput = await Save(OfficeInput, request);
            string tempFilePath = Path.Combine(tempStorage.GetTempPath(officeOutput.Id), officeOutput.FileName);
            DeleteTempFile(tempFilePath);
            return officeOutput;
        }
        /// <summary>
        /// 执行保存和job
        /// </summary>
        /// <param name="OfficeInput"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<OfficeOutput> SaveAndJob(OfficeInput OfficeInput, HttpRequest request)
        {
            TempStorageOperation tempStorage = new TempStorageOperation();
            OfficeOutput officeOutput = await Save(OfficeInput, request);
            string tempFilePath = Path.Combine(tempStorage.GetTempPath(officeOutput.Id), officeOutput.FileName);
            StartJob(officeOutput, tempFilePath);
            return officeOutput;
        }
        /// <summary>
        /// 根据路径打印文件
        /// </summary>
        /// <param name="path"></param>
        public bool PrintByPath(string path)
        {
            PrintJob printJob = new PrintJob();
            JsonFileValues jsonFileValues = printJob.Run(path);
            return true;
        }
        /// <summary>
        /// 保存office文件
        /// </summary>
        /// <param name="OfficeInput"></param>
        /// <returns></returns>
        private async Task<OfficeOutput> Save(OfficeInput OfficeInput, HttpRequest request)
        {
            TempStorageOperation tempStorage = new TempStorageOperation();
            //IStorageService _StorageService = new LocalStorageService();

            //输出结果对象
            OfficeOutput OfficeOutput = new OfficeOutput();
            OfficeOutput.Id = Guid.NewGuid().ToString();
            OfficeOutput.Name = OfficeInput.Name;
            OfficeOutput.Tags = OfficeInput.Tags;
            OfficeOutput.Length = OfficeInput.File.Length;
            OfficeOutput.Type = OfficeInput.File.ContentType;
            OfficeOutput.FileName = OfficeOutput.Id + "." + OfficeInput.File.ContentType.Split("/")[1];
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
                await storageService.CopyDirectory(tempStorage.GetTempPath(OfficeOutput.Id), storageService.GetSavePath(), true);
            }

            //开始处理任务
            //StartJob(OfficeOutput, tempFilePath);

            Console.WriteLine("return:" + DateTime.Now.ToString());
            //返回结果
            return OfficeOutput;
        }
        /// <summary>
        /// 开始处理任务
        /// </summary>
        /// <returns></returns>
        private async Task StartJob(OfficeOutput OfficeOutput, string tempFilePath)
        {
            TempStorageOperation tempStorage = new TempStorageOperation();
            //保存Json文件
            JsonFile jsonFile = new JsonFile();
            jsonFile.Id = OfficeOutput.Id;
            jsonFile.CreateTime = DateTime.Now;
            jsonFile.Name = OfficeOutput.Name;
            jsonFile.Tags = OfficeOutput.Tags;
            jsonFile.Url = OfficeOutput.Url;
            jsonFile.FileName = OfficeOutput.FileName;

            await jsonFile.SaveAs(tempStorage.GetJsonFilePath(jsonFile.Id));
            //执行job
            for (int i = 0; i < jobList.Count; i++)
            {
                jsonFile.Values.Add(jobList[i].Run(tempFilePath));
            }
            //job执行完删除临时文件
            File.Delete(tempFilePath);
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="tempFilePath"></param>
        /// <returns></returns>
        private async Task DeleteTempFile(string tempFilePath)
        {
            File.Delete(tempFilePath);
        }
    }
}
