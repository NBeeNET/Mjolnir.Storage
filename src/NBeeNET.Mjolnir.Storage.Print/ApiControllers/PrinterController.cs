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
    public class PrinterController : ControllerBase
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public PrinterController(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        /// <summary>
        /// 获取所有打印机
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public IActionResult GetPrinters()
        {
            try
            {
                return Ok(PrinterHandleService.GetResource().GetPrinters());
            }
            catch (Exception ex)
            {
                return BadRequest("查询失败！" + ex.ToString());
            }
        }
        /// <summary>
        /// 更新远端打印机
        /// </summary>
        /// <returns></returns>
        [HttpPost()]
        public IActionResult SetPrinters(List<PrinterModel> list)
        {
            try
            {
                PrinterHandleService.GetResource().UpdatePrinterFromRemote(list);
                return Ok("更新打印机完成!");
            }
            catch (Exception ex)
            {
                return BadRequest("更新失败！" + ex.ToString());
            }
        }
    }
}
