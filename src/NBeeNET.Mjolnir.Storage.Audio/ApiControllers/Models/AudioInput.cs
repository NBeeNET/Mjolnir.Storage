using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Audio.ApiControllers.Models
{

    public class AudioInput
    {
        public IFormFile File { get; set; }

        public string Name { get; set; }

        public string Tags { get; set; }
    }
}
