using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Models
{
    /// <summary>
    /// 上传输入
    /// </summary>
    public class UploadInput
    {
        public IFormFile File { get; set; }

        public string Name { get; set; }

        public string Tags { get; set; }
    }
}
