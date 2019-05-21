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
        private JobHandleService _jsonHandleService;
        public JobController()
        {
            _jsonHandleService = new JobHandleService();
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
        /// 添加Job信息
        /// </summary>
        /// <param name="jobsStr">多个Job组合</param>
        /// <returns></returns>
        [HttpPost("Add")]
        public async Task<IActionResult> AddJobs([FromForm]string jobsStr)
        {
            try
            {
                List<JobInput> jobInputModel = null;
                if (string.IsNullOrEmpty(jobsStr))
                {
                    return BadRequest("参数jobInput有误，请检查");
                }
                else
                {
                    jobInputModel = JsonConvert.DeserializeObject<List<JobInput>>(jobsStr);
                }
                if (jobInputModel.Count > 0)
                {
                    var ret = await _jsonHandleService.AddJobs(jobInputModel);
                    return Ok(ret);
                }
                return BadRequest("添加Job失败！");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("添加Job失败！原因：" + ex.Message);
            }
        }

        /// <summary>
        /// 修改Job信息并支持添加文件
        /// </summary>
        /// <param name="jobInput">Job信息</param>
        /// <returns></returns>
        [HttpPost("Modify")]
        public async Task<IActionResult> ModifyJob([FromForm]string jobStr)
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
                //添加文件
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
                var ret = await _jsonHandleService.ModifyJob(jobInputModel);
                return Ok(ret);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("获取Job失败！原因：" + ex.Message);
            }
        }

        /// <summary>
        /// 执行job
        /// </summary>
        /// <param name="jobsStr"></param>
        /// <returns></returns>
        [HttpPost("RunJobs")]
        public IActionResult RunJobs([FromBody]List<JobInput> jobInputs)
        {
            try
            {
   
                if (jobInputs.Count > 0)
                {
                    var ret = _jsonHandleService.RunJobs(jobInputs);
                    return Ok(ret);
                }
                return BadRequest("运行Job失败！");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("添加Job失败！原因：" + ex.Message);
            }
        }


        /// <summary>
        ///执行Job
        /// </summary>
        /// <param name="jobIdInputs"></param>
        /// <returns></returns>
        [HttpPost("Run")]
        public IActionResult RunJobs([FromBody]List<string> jobIdInputs)
        {
            try
            {

                if (jobIdInputs.Count > 0)
                {
                    var ret = _jsonHandleService.RunJobs(jobIdInputs);
                    return Ok(ret);
                }
                return BadRequest("运行Job失败！");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("添加Job失败！原因：" + ex.Message);
            }
        }

    }
}
