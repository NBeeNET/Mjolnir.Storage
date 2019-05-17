using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NBeeNET.Mjolnir.Storage.Print.Models;
using NBeeNET.Mjolnir.Storage.Print.Serivces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NBeeNET.Mjolnir.Storage.Print.ApiControllers
{
    [Route("/StorageApi/[controller]")]
    [ApiController]
    public class OfficeController : ControllerBase
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public OfficeController(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        /// <summary>
        /// 更新打印job状态
        /// </summary>
        /// <returns></returns>
        [HttpPost("{id}")]
        public IActionResult GetJobById(string id)
        {
            try
            {
                return Ok(PrintHandleService.GetResource().GetJobById(id));
            }
            catch (Exception ex)
            {
                return BadRequest("查询失败！" + ex.ToString());
            }
        }
        /// <summary>
        /// 更新打印job状态
        /// </summary>
        /// <returns></returns>
        [HttpPost("Update")]
        public IActionResult UpdateJob(List<PrintJobModel> list)
        {
            try
            {
                PrintHandleService.GetResource().UpdateJobsFromRemote(list);
                return Ok("更新成功");
            }
            catch (Exception ex)
            {
                return BadRequest("更新失败！" + ex.ToString());
            }
        }
        /// <summary>
        /// 删除打印完成的job
        /// </summary>
        /// <param name="ids">id集合</param>
        /// <returns></returns>
        [HttpPost("Delete")]
        public IActionResult DeleteJob(string ids)
        {
            try
            {
                PrintHandleService.GetResource().DeleteJobFromRemote(ids);
                return Ok("删除成功");
            }
            catch (Exception ex)
            {
                return BadRequest("删除失败！" + ex.ToString());
            }
        }

    }
}
