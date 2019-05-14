using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NBeeNET.Mjolnir.Storage.Office.ApiControllers.Models;
using NBeeNET.Mjolnir.Storage.Office.Common;
using NBeeNET.Mjolnir.Storage.Office.Serivces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NBeeNET.Mjolnir.Storage.Image.ApiControllers
{
    [Route("/StorageApi/[controller]")]
    [ApiController]
    public class OfficeController : ControllerBase
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public OfficeController(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }


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
            using (var scope = _serviceScopeFactory.CreateScope())
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
                        var settings = scope.ServiceProvider.GetService<IOptions<Settings>>().Value;
                        if (file.Length >= settings.MaxLength)
                        {
                            string msg = "上传文件的大小超过了最大限制" + settings.MaxLength / 1024 / 1024 + "M";
                            return BadRequest("Office文件上传失败！原因：" + msg);
                        }

                        //验证是否是Office文件
                        if (OfficeValidation.IsCheck(file))
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
                    else
                    {
                        return BadRequest("office文件上传失败！原因：文件大小为0");
                    }
                }
            }
            return BadRequest("office文件上传失败！");
        }


        /// <summary>
        /// 多office文件上传
        /// </summary>
        /// <param name="files">多个图片</param>
        /// <returns></returns>
        [HttpPost("MultipartUpload")]
        public async Task<IActionResult> MultipartUpload(List<IFormFile> files)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
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
                    var settings = scope.ServiceProvider.GetService<IOptions<Settings>>().Value;
                    if (files.Sum(b => b.Length) >= settings.MaxLength)
                    {
                        string msg = "上传文件的总大小超过了最大限制" + settings.MaxLength / 1024 / 1024 + "M";
                        return BadRequest("Office文件上传失败！原因：" + msg);
                    }


                    List<OfficeInput> inputs = new List<OfficeInput>();
                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {
                            //验证是否是Office文件
                            if (OfficeValidation.IsCheck(file))
                            {
                                OfficeInput input = new OfficeInput();
                                input.File = file;
                                input.Name = file.FileName;
                                inputs.Add(input);
                            }
                        }
                    }

                    OfficeHandleService handleService = new OfficeHandleService();
                    //处理office文件
                    var output = await handleService.MultiSaveAndDelete(inputs, Request);

                    return Ok(output);
                }
            }
            return BadRequest("Office文件上传失败！");
        }

    }
}
