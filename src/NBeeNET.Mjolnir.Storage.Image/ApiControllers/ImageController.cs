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
        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImage(IFormFile file,[FromForm]string name, [FromForm]string tags)
        {
            Models.ImageRequest _ImageRequest = new Models.ImageRequest();
            if (file != null)
            {
                if (file.Length > 0)
                {
                    //验证是否是图片
                    if (ImageValidation.IsCheck(file))
                    {
                        _ImageRequest.Id = Guid.NewGuid().ToString();
                        if (string.IsNullOrEmpty(name))
                            _ImageRequest.Name = file.FileName;
                        else
                            _ImageRequest.Name = name;
                        _ImageRequest.Tags = tags;
                        _ImageRequest.Length = file.Length;
                        _ImageRequest.Type = file.ContentType;
                        _ImageRequest.Url = await WriteFile(file);
                        return Ok(_ImageRequest);
                    }
                }
            }
            return BadRequest("图片上传失败！");
        }

        /// <summary>
        /// 多图片上传
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost("UploadImages")]
        public async Task<IActionResult> UploadImages(List<IFormFile> files, [FromForm]string name, [FromForm]string tags)
        {
            List<Models.ImageRequest> _ImageRequests = new List<Models.ImageRequest>();
            Models.ImageRequest _ImageRequest = new Models.ImageRequest();
            if (files.Count > 0)
            {
                foreach (var file in files)
                {
                    _ImageRequest = new Models.ImageRequest();
                    if (file.Length > 0)
                    {
                        if (ImageValidation.IsCheck(file))
                        {
                            _ImageRequest = new Models.ImageRequest();
                            _ImageRequest.Id = Guid.NewGuid().ToString();
                            _ImageRequest.Name = file.FileName;
                            _ImageRequest.Tags = tags;
                            _ImageRequest.Length = file.Length;
                            _ImageRequest.Type = file.ContentType;
                            _ImageRequest.Url = await WriteFile(file);
                            _ImageRequests.Add(_ImageRequest);
                        }
                    }
                }
                return Ok(_ImageRequests);
            }
            
            return BadRequest("图片上传失败！");
            //try
            //{
            //    var result = new UploadResponse();
            //    List<UploadFileEntity> fileEntities = new List<UploadFileEntity>();

            //    //检查是否有文件，没有则从Request.Form.Files获取
            //    if (files.Count == 0)
            //    {
            //        foreach (var item in Request.Form.Files)
            //        {
            //            files.Add(item);
            //        }
            //    }

            //    foreach (var formFile in files)
            //    {
            //        if (formFile.Length > 0)
            //        {
            //            //检查是否是图片
            //            if (CheckIfImageFile(formFile))
            //            {
            //                string guid = Guid.NewGuid().ToString();
            //                string extension = "." + formFile.ContentType.Split("/")[1];
            //                string fileName = guid + extension; //Create a new Name for the file due to security reasons.
            //                string localUrl = Request.Scheme.ToString() + "://" + Request.Host.Value.ToString() + "/sso/images/" + fileName;

            //                T_File _File = new T_File();
            //                _File.Id = guid;
            //                _File.FileName = fileName;
            //                _File.ContentType = formFile.ContentType;
            //                _File.Length = formFile.Length;
            //                _File.SourceUrl = localUrl;
            //                _File.Md5Str = MD5Helper.GetMD5HashFormFile(formFile);
            //                _File.ShortCode = ShortURLHelper.CreateShortURL(localUrl);
            //                _File.ShortUrl = Request.Scheme.ToString() + "://" + Request.Host.Value.ToString() + "/" + _File.ShortCode;
            //                _File.Tags = "";
            //                _File.AppId = "admin";
            //                _File.IpAddress = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
            //                _File.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            //                //写入本地存储
            //                await WriteFile(formFile, fileName);

            //                //写入数据库
            //                await InsertDatabase(_File);

            //                //存入返回结果
            //                fileEntities.Add(new UploadFileEntity()
            //                {
            //                    ContentType = _File.ContentType,
            //                    LocalUrl = _File.SourceUrl,
            //                    ShortUrl = _File.ShortUrl
            //                });
            //            }
            //        }
            //    }

            //    result.Status = "success";
            //    result.Total = files.Count;
            //    result.Files = fileEntities;
            //    return Ok(result);
            //}
            //catch (Exception ex)
            //{
            //    return BadRequest(ex.Message);
            //}

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
