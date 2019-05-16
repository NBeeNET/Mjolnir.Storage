using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NBeeNET.Mjolnir.Storage.Core.ApiControllers
{
    [Route("/StorageApi/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("成功！");
        }
        
    }
}
