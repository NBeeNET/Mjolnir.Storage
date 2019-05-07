using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NBeeNET.Mjolnir.Storage.Image.ApiControllers
{
    [Route("/StorageApi/[controller]")]
    [ApiController]
    public class OfficeController : ControllerBase
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
            return Ok("");
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
            return Ok("");
        }

    }
}
