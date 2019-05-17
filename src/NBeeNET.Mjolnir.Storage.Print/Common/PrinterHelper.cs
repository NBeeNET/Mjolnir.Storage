using NBeeNET.Mjolnir.Storage.Print.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Management;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Print.Common
{
    public static class PrinterHelper
    {
        /// <summary>
        /// 默认打印机名称
        /// </summary>
        public static string DefaultPrinterName { get; set; }
        /// <summary>
        /// 获取默认打印机
        /// </summary>
        /// <returns></returns>
        public static Printer GetDefaultPrinter()
        {
            List<Printer> list = GetPrinterList();
            foreach (Printer _Printer in list)
            {
                if (_Printer.IsDefault)
                    return _Printer;
            }
            return null;
        }
        /// <summary>
        /// 获取打印机列表
        /// </summary>
        /// <returns></returns>
        public static List<Printer> GetPrinterList()
        {
            List<Printer> _PrinterList = new List<Printer>();
            string searchQuery = "SELECT * FROM Win32_Printer";
            //exec WQL
            ManagementObjectSearcher searchPrinters = new ManagementObjectSearcher(searchQuery);
            // get a set of object of managementobject
            ManagementObjectCollection printerCollection = searchPrinters.Get();
            foreach (ManagementObject printer in printerCollection)
            {
                Printer _Printer = new Printer();
                //judge if the current print is the default printer 
                if ((bool)printer.GetPropertyValue("default") == true)
                {
                    _Printer.IsDefault = true;
                }
                _Printer.Name = printer.Properties["Name"].Value.ToString();
                _PrinterList.Add(_Printer);
            }
            return _PrinterList;
        }

        public class Printer
        {
            /// <summary>
            /// 打印机名称
            /// </summary>
            public string Name { get; set; } = string.Empty;

            /// <summary>
            /// 系统默认打印机
            /// </summary>
            public bool IsDefault { get; set; } = false;
        }

        /// <summary>
        /// 获取打印机的详细状态
        /// </summary>
        /// <param name="PrinterName"></param>
        /// <returns></returns>
        public static PrinterState GetPrinterState(string PrinterName)
        {
            string path = @"win32_printer.DeviceId='" + PrinterName + "'";
            ManagementObject printer = new ManagementObject(path);
            printer.Get();
            int intValue = Convert.ToInt32(printer.Properties["PrinterState"].Value);
            return (PrinterState)intValue;
        }

        /// <summary>
        /// 获取打印机的状态信息
        /// </summary>
        /// <param name="PrinterName"></param>
        /// <returns></returns>
        public static PrinterStatus GetPrinterStatus(string PrinterName)
        {
            string path = @"win32_printer.DeviceId='" + PrinterName + "'";
            ManagementObject printer = new ManagementObject(path);
            printer.Get();
            int intValue = Convert.ToInt32(printer.Properties["PrinterStatus"].Value);
            return (PrinterStatus)intValue;
        }

        /// <summary>
        /// 获取打印机配置信息（对象结构暂定）
        /// </summary>
        /// <param name="PrinterName"></param>
        public static void GetPrinterConfiguration(string PrinterName)
        {

            string path = @"win32_printerconfiguration.Name='" + PrinterName + "'";
            ManagementObject printerConfiguration = new ManagementObject(path);
            printerConfiguration.Get();
            foreach (var item in printerConfiguration.Properties)
            {
                Console.WriteLine(item.Name + " : " + item.Value);
            }

        }


        /// <summary>
        /// 获取作业列表
        /// </summary>
        /// <param name="printerName"></param>
        /// <returns></returns>
        public static List<PrintJobModel> GetPrintJobs(string printerName)
        {
            List<PrintJobModel> printJobList = new List<PrintJobModel>();
            string searchQuery = "SELECT * FROM Win32_PrintJob";

            ManagementObjectSearcher searchPrintJobs = new ManagementObjectSearcher(searchQuery);
            ManagementObjectCollection prntJobCollection = searchPrintJobs.Get();
            foreach (ManagementObject prntJob in prntJobCollection)
            {
                foreach (var item in prntJob.Properties)
                {
                    Console.WriteLine(item.Name + " : " + item.Value);
                }

                int jobId = Convert.ToInt32(prntJob.Properties["JobId"].Value);
                string jobName = prntJob.Properties["Name"].Value.ToString();
                string driverName = prntJob.Properties["DriverName"].Value.ToString();
                string documentName = prntJob.Properties["Document"].Value.ToString();
                string jobStatus = prntJob.Properties["JobStatus"].Value.ToString();
                //DateTime TimeSubmitted = (DateTime)prntJob.Properties["JobStatus"].Value;
                string dataType = prntJob.Properties["DataType"].Value.ToString();
                int totalPages = Convert.ToInt32(prntJob.Properties["TotalPages"].Value);
                int size = Convert.ToInt32(prntJob.Properties["Size"].Value);
                int pagesPrinted = Convert.ToInt32(prntJob.Properties["PagesPrinted"].Value);

                if (String.Compare(driverName, printerName, true) == 0)
                {
                    var printJob = new PrintJobModel()
                    {
                        JobId = jobId,
                        Name = jobName,
                        Document = documentName,
                        DriverName = driverName,
                        JobStatus = jobStatus,
                        //TimeSubmitted = TimeSubmitted,
                        DataType = dataType,
                        TotalPages = totalPages,
                        Size = size,
                        PagesPrinted = pagesPrinted
                    };
                    printJobList.Add(printJob);
                }
            }
            return printJobList;
        }

    }


    /// <summary>
    /// 打印机的状态信息
    /// </summary>
    public enum PrinterStatus
    {
        [Description("其他")]
        Other = 1,

        [Description("未知")]
        Unknown = 2,

        [Description("空闲")]
        Idle = 3,

        [Description("正在打印")]
        Printing = 4,

        [Description("热机中")]
        Warmup = 5,

        [Description("已停止打印")]
        Stopped_Printing = 6,

        [Description("脱机")]
        Offline = 7
    }

    /// <summary>
    /// 与此打印机相关的可能状态
    /// </summary>
    public enum PrinterState
    {
        [Description("空闲")]
        Idle = 0,

        [Description("暂停")]
        Paused = 1,

        [Description("错误")]
        Error = 2,

        [Description("正在删除")]
        Pending_Deletion = 3,

        [Description("塞纸")]
        Paper_Jam = 4,

        [Description("打印纸用完")]
        Paper_Out = 5,

        [Description("手动送纸")]
        Manual_Feed = 6,

        [Description("纸张问题")]
        Paper_Problem = 7,

        [Description("脱机")]
        Offline = 8,

        [Description("正在输入输出")]
        IO_Active = 9,

        [Description("忙碌")]
        Busy = 10,

        [Description("正在打印")]
        Printing = 11,

        [Description("输出口已满")]
        Output_Bin_Full = 12,

        [Description("不可用")]
        Not_Available = 13,

        [Description("等待")]
        Waiting = 14,

        [Description("正在处理")]
        Processing = 15,

        [Description("初始化")]
        Initialization = 16,

        [Description("热机中")]
        Warming_Up = 17,

        [Description("墨粉不足")]
        Toner_Low = 18,

        [Description("无墨粉")]
        No_Toner = 19,

        [Description("当前页无法打印")]
        Page_Punt = 20,

        [Description("需要用户干预")]
        User_Intervention_Required = 21,

        [Description("内存溢出")]
        Out_Of_Memory = 22,

        [Description("被打开")]
        Door_Open = 23,

        [Description("未知状态")]
        Server_Unknown = 24,

        [Description("省点模式")]
        Power_Save = 25
    }
}
