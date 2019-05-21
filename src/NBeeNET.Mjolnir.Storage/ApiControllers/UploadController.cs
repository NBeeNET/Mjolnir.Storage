using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NBeeNET.Mjolnir.Storage.ApiControllers
{
    [Route("/StorageApi/")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public UploadController(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        /// <summary>
        /// 单文件上传
        /// </summary>
        /// <param name="file">文件</param>
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
                var settings = scope.ServiceProvider.GetService<IOptions<Settings>>().Value;
                if (file.Length >= settings.MaxLength)
                {
                    string msg = "上传文件的大小超过了最大限制" + settings.MaxLength / 1024 / 1024 + "M";
                    return BadRequest("图片上传失败！原因：" + msg);
                }
                Models.UploadOutput output = new Models.UploadOutput();
                if (file != null)
                {
                    if (file.Length > 0)
                    {
                        Models.UploadInput input = new Models.UploadInput();
                        input.File = file;
                        input.Name = string.IsNullOrEmpty(name) ? file.FileName : name;
                        input.Tags = tags;
                        UploadHandleService handleService = new UploadHandleService();
                        output = await handleService.Save(input, Request);
                        return Ok(output);
                    }
                }
            }
            return BadRequest("上传失败！");
        }

        /// <summary>
        /// 多文件上传
        /// </summary>
        /// <param name="files">多个文件</param>
        /// <param name="name">自定义名称</param>
        /// <param name="file">自定义Tag</param>
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
                        return BadRequest("文件上传失败！原因：" + msg);
                    }

                    List<Models.UploadInput> inputs = new List<Models.UploadInput>();
                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {

                            Models.UploadInput input = new Models.UploadInput();
                            input.File = file;
                            input.Name = file.Name;
                            input.Tags = file.Name;
                            inputs.Add(input);
                        }
                    }

                    UploadHandleService handleService = new UploadHandleService();
                    var output = await handleService.MultiSave(inputs, Request);
                    return Ok(output);
                }
            }
            return BadRequest("上传失败！");
        }

    }
}
