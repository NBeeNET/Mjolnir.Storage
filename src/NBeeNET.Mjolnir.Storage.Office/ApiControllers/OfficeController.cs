using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NBeeNET.Mjolnir.Storage.Office.ApiControllers.Models;
using NBeeNET.Mjolnir.Storage.Office.Serivces;

namespace NBeeNET.Mjolnir.Storage.Image.ApiControllers
{
    [Route("/StorageApi/[controller]")]
    [ApiController]
    public class OfficeController : ControllerBase
    {
        /// <summary>
        /// office文件上传
        /// </summary>
        /// <param name="file">office文件</param>
        /// <param name="name">自定义名称</param>
        /// <param name="file">自定义Tag</param>
        /// <returns></returns>
        [HttpPost("Upload")]
        public async Task<IActionResult> Upload(IFormFile file, [FromForm]string name, [FromForm]string tags)
        {
            if (file == null)
            {
                file = Request.Form.Files[0];
            }
            OfficeOutput output = new OfficeOutput();
            if (file != null)
            {
                if (file.Length > 0)
                {
                    OfficeInput input = new OfficeInput();
                    input.File = file;
                    input.Name = string.IsNullOrEmpty(name) ? file.FileName : name;
                    input.Tags = tags;
                    OfficeHandleService handleService = new OfficeHandleService();
                    //处理office文件
                    output = await handleService.SaveAndDelete(input, Request);

                    return Ok(output);
                }
            }
            return BadRequest("office文件上传失败！");
        }

        /// <summary>
        /// office文件上传打印
        /// </summary>
        /// <param name="file">office文件</param>
        /// <param name="name">自定义名称</param>
        /// <param name="file">自定义Tag</param>
        /// <returns></returns>
        [HttpPost("UploadPrint")]
        public async Task<IActionResult> UploadPrint(IFormFile file, [FromForm]string name, [FromForm]string tags)
        {
            if (file == null)
            {
                file = Request.Form.Files[0];
            }
            OfficeOutput output = new OfficeOutput();
            if (file != null)
            {
                if (file.Length > 0)
                {
                    OfficeInput input = new OfficeInput();
                    input.File = file;
                    input.Name = string.IsNullOrEmpty(name) ? file.FileName : name;
                    input.Tags = tags;
                    OfficeHandleService handleService = new OfficeHandleService();
                    //处理office文件
                    output = await handleService.SaveAndJob(input, Request);

                    return Ok(output);
                }
            }
            return BadRequest("office文件上传失败！");
        }
        /// <summary>
        /// 根据路径打印
        /// </summary>
        /// <param name="file">office文件</param>
        /// <returns></returns>
        [HttpPost("Print")]
        public IActionResult Print(string path)
        {
            OfficeHandleService handleService = new OfficeHandleService();
            //打印office文件
            if (handleService.PrintByPath(path))
            {
                return Ok(true);
            }
            return BadRequest("office打印失败！");
        }
    }
}
