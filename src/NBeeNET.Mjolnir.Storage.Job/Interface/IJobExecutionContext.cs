using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Job.Interface
{
    /// <summary>
    /// 包含各种环境信息句柄的上下文包
    /// </summary>
    public interface IJobExecutionContext
    {
        IScheduler Scheduler { get; }

        IJobDetail JobDetail { get; }

        Hashtable MergedJobDataMap { get; }

        IJob JobInstance { get; }

        object Result { get; set; }


    }
}
