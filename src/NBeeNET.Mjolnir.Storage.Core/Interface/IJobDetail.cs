using NBeeNET.Mjolnir.Storage.Core.Implement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Core.Interface
{
    /// <summary>
    /// 作业实例的详细属性,将使用<see cref=“JobBuilder”/>创建/定义
    /// </summary>
    public interface IJobDetail
    {

        /// <summary>
        /// Job的唯一值
        /// </summary>
        string Key { get;  }

        /// <summary>
        /// Job名称
        /// </summary>
        string Name { get;  }
        /// <summary>
        /// 获取或者设置Job的详细描述 
        /// creator (if any).
        /// </summary>
        string Description { get;  }

        /// <summary>
        ///获取或者设置Job的类型
        /// </summary>
        Type JobType { get;  }

        Hashtable JobDataMap { get; }

        /// <summary>
        /// 获取一下 <see cref="JobBuilder" /> that is configured to produce a 
        /// <see cref="IJobDetail" /> identical to this one.
        /// </summary>
        JobBuilder GetJobBuilder();

        /// <summary>
        /// 克隆一个相同IJobDetail对象
        /// </summary>
        /// <returns></returns>
        IJobDetail Clone();

    }
}
