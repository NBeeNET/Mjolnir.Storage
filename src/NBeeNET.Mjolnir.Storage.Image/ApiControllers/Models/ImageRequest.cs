using System;
using System.Collections.Generic;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Image.ApiControllers.Models
{
    public class ImageRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Tags { get; set; }
        public long Length { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public string Path { get; set; }
    }
}
