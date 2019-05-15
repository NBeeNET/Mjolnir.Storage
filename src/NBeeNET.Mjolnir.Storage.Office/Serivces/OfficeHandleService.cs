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
        /// 文件保存
        /// </summary>
        /// <param name="OfficeInput"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<OfficeOutput> SaveOfficeFile(OfficeInput OfficeInput, HttpRequest request)
        {
            TempStorageOperation tempStorage = new TempStorageOperation();
            OfficeOutput officeOutput = await Save(OfficeInput, request);
            return officeOutput;
        }

        /// <summary>
        /// 多文件保存
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<OfficeOutput>> MultiSaveOfficeFile(List<OfficeInput> inputs, HttpRequest request)
        {
            List<OfficeOutput> output = new List<OfficeOutput>();
            for (int i = 0; i < inputs.Count; i++)
            {
                var result = await SaveOfficeFile(inputs[i], request);
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


            //保存Json文件
            JsonFile jsonFile = new JsonFile();
            jsonFile.Id = OfficeOutput.Id;
            jsonFile.CreateTime = DateTime.Now;
            jsonFile.Name = OfficeOutput.Name;
            jsonFile.Tags = OfficeOutput.Tags;
            jsonFile.Url = OfficeOutput.Url;
            jsonFile.FileName = OfficeOutput.FileName;
       
            await jsonFile.SaveAs(tempStorage.GetJsonFilePath(jsonFile.Id));

            //删除临时目录
            tempStorage.Delete(jsonFile.Id);
            Console.WriteLine("return:" + DateTime.Now.ToString());
            //返回结果
            return OfficeOutput;
        }
        

    }
}
