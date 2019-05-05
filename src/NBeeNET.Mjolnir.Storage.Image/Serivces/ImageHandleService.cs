using Microsoft.AspNetCore.Http;
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
        public IFormFile formFile = null;

        public ImageHandleService()
        {

        }

        public ImageHandleService(IFormFile file)
        {
            formFile = file;
        }


        /// <summary>
        /// 创建临时文件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> CreateTempFile(string id)
        {
            return await new Core.TempFileOperation().WriteTempFile(formFile, id,"");
        }

        /// <summary>
        /// 创建Json文件
        /// </summary>
        /// <param name="imageRequest"></param>
        /// <returns></returns>
        public async Task CreateJsonFile(ImageRequest imageRequest)
        {
            FileStrorageModel model = new FileStrorageModel();
            model.Id = imageRequest.Id;
            model.Name = imageRequest.Name;
            model.PathUrl = imageRequest.Path;
            model.StartTime = DateTime.MinValue;
            List<FileStrorageTaskModel> tasks = new List<FileStrorageTaskModel>();
            tasks.Add(new FileStrorageTaskModel() { Name = "MakeThumbnail", Param = "{ width:100; height:100;mode:'W'}", Status = "" });
            model.Tasks = tasks;
            string JsonStr = JsonConvert.SerializeObject(model);
            new Core.TempFileOperation().WriteJsonFile(model.Id, JsonStr);
        }

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="tempFilePath"></param>
        /// <returns></returns>
        public async Task<bool> SaveImage(string tempFilePath)
        {
            //写入临时文件
            if (tempFilePath != "")
            {
                var tempFile = new FileInfo(tempFilePath);
                var id = tempFile.Name.Replace(tempFile.Extension, "");

                string sourceDir = new FileInfo(tempFilePath).DirectoryName;

                string rootDirectory = Directory.GetCurrentDirectory() + "\\wwwroot\\NBeeNET\\";
                if (!Directory.Exists(rootDirectory))
                {
                    Directory.CreateDirectory(rootDirectory);
                }
                string destinationDir = rootDirectory + "\\Images\\" + id + "\\";
                if (!Directory.Exists(destinationDir))
                {
                    Directory.CreateDirectory(destinationDir);
                }
                //复制到目录
                IStorageService storageService = new LocalStorageService();
                return await storageService.CopyDirectory(sourceDir, destinationDir, true);

            }
            return false;
        }

        /// <summary>
        /// 文件处理队列
        /// </summary>
        private Queue<FileStrorageTaskModel> _ProcessingQueue = new Queue<FileStrorageTaskModel>();

        /// <summary>
        /// 处理图片
        /// </summary>
        /// <returns></returns>

        public void Start()
        {

        }
    }
}
