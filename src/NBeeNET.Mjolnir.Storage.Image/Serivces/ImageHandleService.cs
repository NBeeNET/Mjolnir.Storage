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
      

        public ImageHandleService()
        {

        }


        /// <summary>
        /// 处理图片
        /// </summary>
        /// <param name="tempFilePath"></param>
        /// <returns></returns>
        public async Task<bool> Processing(IFormFile file, string id)
        {
            //写入临时文件

            //写入Json文件

            //处理任务

            //更新Json文件

            //删除临时文件
            return false;
        }


       
        /// <summary>
        /// 文件处理队列
        /// </summary>
        private Queue<T> _ProcessingQueue = new Queue<T>();

        /// <summary>
        /// 处理图片
        /// </summary>
        /// <returns></returns>

        public void Start()
        {

        }
    }
}
