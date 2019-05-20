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
        public static PrinterModel GetDefaultPrinter()
        {
            List<PrinterModel> list = GetPrinterList(); 
            foreach (PrinterModel _Printer in list)
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
        public static List<PrinterModel> GetPrinterList()
        {
            List<PrinterModel> _PrinterList = new List<PrinterModel>();
            string searchQuery = "SELECT * FROM Win32_Printer";
            //exec WQL
            ManagementObjectSearcher searchPrinters = new ManagementObjectSearcher(searchQuery);
            // get a set of object of managementobject
            ManagementObjectCollection printerCollection = searchPrinters.Get();
            foreach (ManagementObject printer in printerCollection)
            {
                foreach (var item in printer.Properties)
                {
                    Console.WriteLine(item.Name + " : " + item.Value);
                }
                PrinterModel _Printer = new PrinterModel();
                //judge if the current print is the default printer 
                //if ((bool)printer.GetPropertyValue("default") == true)
                //{
                //    _Printer.IsDefault = true;
                //}
                _Printer.Name = printer.Properties["Name"].Value.ToString();
                _Printer.IsDefault = (bool)printer.GetPropertyValue("default");
                _Printer.PrinterState = (PrinterState)Convert.ToInt32(printer.Properties["PrinterState"].Value);
                _Printer.PrinterStatus = (PrinterStatus)Convert.ToInt32(printer.Properties["PrinterStatus"].Value);
                _PrinterList.Add(_Printer);
            }
            return _PrinterList;
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
        /// 获取所有作业列表
        /// </summary>
        /// <param name="printerName"></param>
        /// <returns></returns>
        public static List<PrintJobModel> GetPrintJobs()
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
                string printerName = jobName.Split(',')[0];
                string driverName = prntJob.Properties["DriverName"].Value.ToString();
                string documentName = prntJob.Properties["Document"].Value.ToString();
                string jobStatus = prntJob.Properties["JobStatus"].Value.ToString();
                //DateTime TimeSubmitted = (DateTime)prntJob.Properties["JobStatus"].Value;
                string dataType = prntJob.Properties["DataType"].Value.ToString();
                int totalPages = Convert.ToInt32(prntJob.Properties["TotalPages"].Value);
                int size = Convert.ToInt32(prntJob.Properties["Size"].Value);
                //int pagesPrinted = Convert.ToInt32(prntJob.Properties["PagesPrinted"].Value);

                var printJob = new PrintJobModel()
                {
                    JobId = jobId,
                    Name = jobName,
                    PrinterName = printerName,
                    Document = documentName,
                    DriverName = driverName,
                    JobStatus = jobStatus,
                    //TimeSubmitted = TimeSubmitted,
                    DataType = dataType,
                    TotalPages = totalPages,
                    Size = size
                };
                printJobList.Add(printJob);
            }
            return printJobList;
        }
        /// <summary>
        /// 获取指定打印机作业列表
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
                string printer = jobName.Split(',')[0];
                string driverName = prntJob.Properties["DriverName"].Value.ToString();
                string documentName = prntJob.Properties["Document"].Value.ToString();
                string jobStatus = prntJob.Properties["JobStatus"].Value.ToString();
                //DateTime TimeSubmitted = (DateTime)prntJob.Properties["JobStatus"].Value;
                string dataType = prntJob.Properties["DataType"].Value.ToString();
                int totalPages = Convert.ToInt32(prntJob.Properties["TotalPages"].Value);
                int size = Convert.ToInt32(prntJob.Properties["Size"].Value);
                //int pagesPrinted = Convert.ToInt32(prntJob.Properties["PagesPrinted"].Value);

                if (jobName.StartsWith(printerName + ","))
                {
                    var printJob = new PrintJobModel()
                    {
                        JobId = jobId,
                        Name = jobName,
                        PrinterName = printer,
                        Document = documentName,
                        DriverName = driverName,
                        JobStatus = jobStatus,
                        //TimeSubmitted = TimeSubmitted,
                        DataType = dataType,
                        TotalPages = totalPages,
                        Size = size
                    };
                    printJobList.Add(printJob);
                }
            }
            return printJobList;
        }

    }
}
