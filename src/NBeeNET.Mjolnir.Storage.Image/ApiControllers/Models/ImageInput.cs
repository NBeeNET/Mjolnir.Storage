using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Image.ApiControllers.Models
{

    public class ImageInput
    {
        public IFormFile File { get; set; }

        public string Name { get; set; }

        public string Tags { get; set; }
    }
}
