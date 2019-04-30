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
    [Route("/NBeeNET/Mjolnir.Storage/Api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        /// <summary>
        /// 图片上传
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost,HttpPut,HttpPost,HttpPatch]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            //验证是否是图片
            if (ImageValidation.IsCheck(file))
            {
                var result = await WriteFile(file);
                return new ObjectResult(result);
            }

            return BadRequest("Invalid image file");
        }
        
        /// <summary>
        /// Method to write file onto the disk
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private async Task<string> WriteFile(IFormFile file)
        {
            string fileName;
            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                fileName = Guid.NewGuid().ToString() + extension; //Create a new Name for the file due to security reasons.
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);

                using (var bits = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(bits);
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }

            return fileName;
        }
        
    }
}
