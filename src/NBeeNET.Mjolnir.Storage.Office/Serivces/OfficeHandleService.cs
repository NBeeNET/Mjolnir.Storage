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
        /// 多文件 执行保存和删除
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<OfficeOutput>> MultiSaveAndDelete(List<OfficeInput> inputs, HttpRequest request)
        {
            List<OfficeOutput> output = new List<OfficeOutput>();
            for (int i = 0; i < inputs.Count; i++)
            {
                var result = await SaveAndDelete(inputs[i], request);
                output.Add(result);
            }
            return output;
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

            
            Console.WriteLine("return:" + DateTime.Now.ToString());
            //返回结果
            return OfficeOutput;
        }
        
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="tempFilePath"></param>
        /// <returns></returns>
        private void DeleteTempFile(string tempFilePath)
        {
            File.Delete(tempFilePath);
        }
    }
}
