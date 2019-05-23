using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Print.Models
{
    public class PrinterModel
    {
        public void CopyFrom(PrinterModel  printerModel)
        {
            this.IsDefault = printerModel.IsDefault;
            this.PrinterState = printerModel.PrinterState;
            this.PrinterStatus = printerModel.PrinterStatus;
            this.IsLocal = printerModel.IsLocal;
            this.ModifyTime = DateTime.Now;
        }
        /// <summary>
        /// 打印机名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 系统默认打印机
        /// </summary>
        public bool IsDefault { get; set; } = false;
        /// <summary>
        /// 打印机状态
        /// </summary>
        public string PrinterState { get; set; }
        /// <summary>
        /// 打印机打印状态
        /// </summary>
        public PrinterStatus PrinterStatus { get; set; }
        /// <summary>
        /// 是否为本地打印机
        /// </summary>
        public bool IsLocal { get; set; } = true;
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime ModifyTime { get; set; } = DateTime.Now;
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

        [Description("省电模式")]
        Power_Save = 25
    }
}
