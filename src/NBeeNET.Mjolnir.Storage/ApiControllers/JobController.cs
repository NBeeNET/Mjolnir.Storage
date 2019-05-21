using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NBeeNET.Mjolnir.Storage.Core.Models;
using NBeeNET.Mjolnir.Storage.Models;
using NBeeNET.Mjolnir.Storage.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NBeeNET.Mjolnir.Storage.ApiControllers
{
    [Route("/StorageApi/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private JsonHandleService _jsonHandleService;
        public JobController()
        {
            _jsonHandleService = new JsonHandleService();
        }


        /// <summary>
        /// 获取Job信息
        /// </summary>
        /// <param name="id">过滤Id</param>
        /// <param name="key">过滤Job Key</param>
        /// <param name="state">过滤Job State</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetJobs(string id = "", string key = "", string state = "")
        {
            try
            {
                var ret = _jsonHandleService.GetJobs(id, key, state);
                return Ok(ret);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("获取Job失败！");
            }

        }

        /// <summary>
        /// 设置Job信息
        /// </summary>
        /// <param name="jobInput">Job信息</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SetJob([FromForm]string jobStr)
        {
            try
            {
                JobInput jobInputModel = null;
                if (string.IsNullOrEmpty(jobStr))
                {
                    return BadRequest("参数jobInput有误，请检查");
                }
                else
                {
                    jobInputModel = JsonConvert.DeserializeObject<JobInput>(jobStr);
                }
                if (jobInputModel.Files != null && jobInputModel.Files.Count == 0)
                {
                    if (Request.Form.Files.Count > 0)
                    {
                        jobInputModel.Files = new List<IFormFile>();
                        foreach (var item in Request.Form.Files)
                        {
                            jobInputModel.Files.Add(item);
                        }
                    }
                }
                var ret = await _jsonHandleService.SetJob(jobInputModel);
                return Ok(ret);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("获取Job失败！原因：" + ex.Message);
            }
        }

    }
}
