using Microsoft.AspNetCore.Http;
using NBeeNET.Mjolnir.Storage.Print.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace NBeeNET.Mjolnir.Storage.Print.Serivces
{
    /// <summary>
    /// Office处理
    /// </summary>
    public class PrinterHandleService
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        private PrinterHandleService()
        {
        }
        public static PrinterHandleService GetResource()
        {
            if (_printerHandleService == null)
                _printerHandleService = new PrinterHandleService();
            return _printerHandleService;
        }

        private static PrinterHandleService _printerHandleService = null;

        /// <summary>
        /// 打印机集合
        /// </summary>
        private List<PrinterModel> printerModels = new List<PrinterModel>();


        /// <summary>
        /// 获取所有打印机
        /// </summary>
        /// <param name="id"></param>
        /// <param name="printName">打印机名称</param>
        /// <param name="isRemote">是否远端任务</param>
        /// <returns></returns>
        public List<PrinterModel> GetPrinters()
        {
            UpdatePrinterFromLocal();
            return printerModels;
        }

        /// <summary>
        /// 更新远端打印机
        /// </summary>
        public void UpdatePrinterFromRemote(List<PrinterModel> list)
        {
            lock (printerModels)
            {
                foreach (PrinterModel printerModel in list)
                {
                    printerModel.IsLocal = false;
                    PrinterModel jobModel = printerModels.Find(obj => obj.Name == printerModel.Name && !obj.IsLocal);
                    if (jobModel == null)
                        printerModels.Add(printerModel);
                    else
                        jobModel.CopyFrom(printerModel);
                }
            }
        }
        /// <summary>
        /// 更新本地打印机
        /// </summary>
        private void UpdatePrinterFromLocal()
        {
            lock (printerModels)
            {
                //删除长时间不更新的打印机
                List<PrinterModel> _delList = printerModels.FindAll(obj => obj.ModifyTime.AddDays(1) < DateTime.Now);
                foreach (PrinterModel printerModel in _delList)
                {
                    printerModels.Remove(printerModel);
                }
                //获取本地打印机
                List<PrinterModel> list = Print.Common.PrinterHelper.GetPrinterList();
                foreach (PrinterModel printerModel in list)
                {
                    var data = printerModels.Find(obj => obj.Name == printerModel.Name && obj.IsLocal);
                    if (data != null)
                        printerModels.Add(printerModel); 
                    else
                        data.CopyFrom(printerModel);
                }
            }
        }
    }
}
