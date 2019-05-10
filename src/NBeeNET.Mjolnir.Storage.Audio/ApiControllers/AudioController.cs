using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NBeeNET.Mjolnir.Storage.Audio.ApiControllers.Models;
using NBeeNET.Mjolnir.Storage.Audio.Serivces;

namespace NBeeNET.Mjolnir.Storage.Audio.ApiControllers
{
    [Route("/StorageApi/[controller]")]
    [ApiController]
    public class AudioController : ControllerBase
    {
        /// <summary>
        /// 音频上传
        /// </summary>
        /// <param name="file">音频</param>
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
                return BadRequest("音频上传失败！原因：" + msg);
            }
            AudioOutput output = new AudioOutput();
            if (file != null)
            {
                if (file.Length > 0)
                {
                    //验证是否是音频
                    if (AudioValidationClass.IsCheck(file))
                    {
                        AudioInput input = new AudioInput();
                        input.File = file;
                        input.Name = string.IsNullOrEmpty(name) ? file.FileName : name;
                        input.Tags = tags;
                        AudioHandleService handleService = new AudioHandleService();
                        //处理音频
                        output = await handleService.Processing(input, Request);

                        return Ok(output);
                    }
                }
            }
            return BadRequest("音频上传失败！");
        }

        /// <summary>
        /// 多音频上传
        /// </summary>
        /// <param name="files">多个音频</param>
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
                    return BadRequest("音频上传失败！原因：" + msg);
                }

                List<AudioInput> inputs = new List<AudioInput>();
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        //验证是否是音频
                        if (AudioValidationClass.IsCheck(file))
                        {
                            AudioInput input = new AudioInput();
                            input.File = file;
                            input.Name = file.Name;
                            input.Tags = file.Name;
                            inputs.Add(input);
                        }
                    }
                }

                AudioHandleService handleService = new AudioHandleService();
                //处理音频
                var output = await handleService.ProcessingImages(inputs, Request);
                return Ok(output);
            }
            return BadRequest("inputs上传失败！");
        }

    }
}
