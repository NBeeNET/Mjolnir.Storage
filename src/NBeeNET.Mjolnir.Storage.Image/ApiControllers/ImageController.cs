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
    [Route("/NBeeNET/Mjolnir.Storage/Api/[controller]")]
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
        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImage(IFormFile file, [FromForm]string name, [FromForm]string tags)
        {
            if (file==null)
            {
                file = Request.Form.Files[0];
            }
            ImageOutput output = new ImageOutput();
            if (file != null)
            {
                if (file.Length > 0)
                {
                    //验证是否是图片
                    if (ImageValidation.IsCheck(file))
                    {
                        ImageInput input = new ImageInput();
                        input.File = file;
                        input.Name = name;
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
        [HttpPost("UploadImages")]
        public async Task<IActionResult> UploadImages(List<IFormFile> files)
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
            if (files.Count>0)
            {
                List<ImageInput> imageInputs = new List<ImageInput>();
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        //验证是否是图片
                        if (ImageValidation.IsCheck(file))
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
