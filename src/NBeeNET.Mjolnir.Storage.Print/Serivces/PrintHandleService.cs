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
    public class PrintHandleService
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        private PrintHandleService()
        {
        }
        public static PrintHandleService GetResource()
        {
            if (_printHandleService == null)
                _printHandleService = new PrintHandleService();
            return _printHandleService;
        }

        private static PrintHandleService _printHandleService = null;

        /// <summary>
        /// 打印job状态集合
        /// </summary>
        private List<PrintJobModel> printJobModels = new List<PrintJobModel>();


        /// <summary>
        /// 根据id获取指定job状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="printName">打印机名称</param>
        /// <param name="isRemote">是否远端任务</param>
        /// <returns></returns>
        public List<PrintJobModel> GetJobById(string id= null,string printName = null,bool isRemote = false)
        {
            if(!isRemote)
                UpdateJobsFromLocal(printName);
            if (string.IsNullOrEmpty(id))
                return printJobModels;
            return printJobModels.FindAll(obj => obj.Document.Contains(id));
        }

        /// <summary>
        /// 更新或添加jobList(远端用)
        /// </summary>
        public void UpdateJobsFromRemote(List<PrintJobModel> list)
        {
            lock (printJobModels)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    PrintJobModel jobModel = printJobModels.Find(obj => list[i].Document.Contains(obj.Document));
                    if (jobModel == null)
                        printJobModels.Add(list[i]);
                    else
                    {
                        jobModel.CopyFrom(list[i]);
                        jobModel.DataFrom = "远端";
                    }
                }
            }
        }

        /// <summary>
        /// 更新.添加.删除jobList(本地用)
        /// </summary>
        public void UpdateJobsFromLocal(string printerName = null)
        {
            lock (printJobModels)
            {
                try
                {
                    //删除1天内没更新状态的job
                    List<PrintJobModel> _delList = printJobModels.FindAll(obj => obj.TimeSubmitted.AddDays(1) < DateTime.Now);
                    for (int i = 0; i < _delList.Count; i++)
                    {
                        printJobModels.Remove(_delList[i]);
                    }

                    if (string.IsNullOrEmpty(printerName))
                    {
                        PrinterModel printer = Print.Common.PrinterHelper.GetDefaultPrinter();
                        printerName = printer.Name;
                    }
                    List<PrintJobModel> _List = Print.Common.PrinterHelper.GetPrintJobs();
                    foreach (PrintJobModel printJobModel in _List)
                    {
                        var data = printJobModels.Find(obj => printJobModel.Document.Contains(obj.Document));
                        if (data != null)
                        {
                            //更新
                            data.CopyFrom(printJobModel);
                            data.DataFrom = "本地";
                        }
                        else
                        {
                            //添加
                            printJobModels.Add(printJobModel);
                            printJobModel.DataFrom = "本地";
                        }
                    }
                    //当前打印机缓存的job
                    List<PrintJobModel> printerJob = printJobModels.FindAll(obj => obj.DataFrom == "本地");
                    foreach (PrintJobModel printJobModel in printerJob)
                    {
                        var data = _List.Find(obj => obj.Document.Contains(printJobModel.Document));
                        if (data == null)
                        {
                            printJobModel.JobStatus = "打印完成";
                            printJobModel.ModifyTime = DateTime.Now;
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
