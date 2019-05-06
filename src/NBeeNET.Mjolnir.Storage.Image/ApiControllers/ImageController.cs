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
        //[HttpPost("UploadImages")]
        //public async Task<IActionResult> UploadImages(List<IFormFile> files, [FromForm]string name, [FromForm]string tags)
        //{
        //    List<Models.ImageRequest> _ImageRequests = new List<Models.ImageRequest>();
        //    Models.ImageRequest _ImageRequest = new Models.ImageRequest();
        //    if (files.Count > 0)
        //    {
        //        foreach (var file in files)
        //        {
        //            _ImageRequest = new Models.ImageRequest();
        //            if (file.Length > 0)
        //            {
        //                //验证是否是图片
        //                if (ImageValidation.IsCheck(file))
        //                {
        //                    _ImageRequest = new Models.ImageRequest();
        //                    _ImageRequest.Id = Guid.NewGuid().ToString();
        //                    _ImageRequest.Name = file.FileName;
        //                    _ImageRequest.Tags = tags;
        //                    _ImageRequest.Length = file.Length;
        //                    _ImageRequest.Type = file.ContentType;
        //                    _ImageRequest.Url = await new Core.TempStorageOperation().Write(file, _ImageRequest.Id);
        //                    _ImageRequests.Add(_ImageRequest);
        //                }
        //            }
        //        }
        //        return Ok(_ImageRequests);
        //    }

        //    return BadRequest("图片上传失败！");
        //}

    }
}
