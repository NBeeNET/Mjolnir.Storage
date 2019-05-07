using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NBeeNET.Mjolnir.Storage.Image.ApiControllers.Models;
using NBeeNET.Mjolnir.Storage.Image.Serivces;

namespace NBeeNET.Mjolnir.Storage.Image.ApiControllers
{
    [Route("/StorageApi/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        /// <summary>
        /// 图片上传
        /// </summary>
        /// <param name="file">图片</param>
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

            if (file.Length >= Operation._OperationValues.MaxLength)
            {
                string msg = "上传文件的大小超过了最大限制" + Operation._OperationValues.MaxLength / 1024 / 1024 + "M";
                return BadRequest("图片上传失败！原因：" + msg);
            }
            ImageOutput output = new ImageOutput();
            if (file != null)
            {
                if (file.Length > 0)
                {
                    //验证是否是图片
                    if (ImageValidationClass.IsCheck(file))
                    {
                        ImageInput input = new ImageInput();
                        input.File = file;
                        input.Name = string.IsNullOrEmpty(name) ? file.FileName : name;
                        input.Tags = tags;
                        ImageHandleService handleService = new ImageHandleService();
                        //处理图片
                        output = await handleService.Processing(input, Request);

                        return Ok(output);
                    }
                }
            }
            return BadRequest("图片上传失败！");
        }

        /// <summary>
        /// 多图片上传
        /// </summary>
        /// <param name="files">多个图片</param>
        /// <param name="name">自定义名称</param>
        /// <param name="file">自定义Tag</param>
        /// <returns></returns>
        [HttpPost("MultipartUpload")]
        public async Task<IActionResult> MultipartUpload(List<IFormFile> files)
        {

            if (files.Count == 0)
            {
                if (Request.Form.Files.Count > 0)
                {
                    foreach (var item in Request.Form.Files)
                    {
                        files.Add(item);
                    }
                }
            }
            if (files.Count > 0)
            {

                if (files.Sum(b => b.Length) >= Operation._OperationValues.MaxLength)
                {
                    string msg = "上传文件的总大小超过了最大限制" + Operation._OperationValues.MaxLength / 1024 / 1024 + "M";
                    return BadRequest("图片上传失败！原因：" + msg);
                }

                List<ImageInput> imageInputs = new List<ImageInput>();
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        //验证是否是图片
                        if (ImageValidationClass.IsCheck(file))
                        {
                            ImageInput input = new ImageInput();
                            input.File = file;
                            input.Name = file.Name;
                            input.Tags = file.Name;
                            imageInputs.Add(input);
                        }
                    }
                }

                ImageHandleService handleService = new ImageHandleService();
                //处理图片
                var output = await handleService.ProcessingImages(imageInputs, Request);
                return Ok(output);
            }
            return BadRequest("图片上传失败！");
        }

    }
}
