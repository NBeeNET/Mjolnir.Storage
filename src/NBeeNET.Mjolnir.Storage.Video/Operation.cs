using System;
using System.Collections.Generic;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Video
{
    public class Operation
    {
        public static OperationValues _OperationValues { get; set; }

        public static void Set(OperationValues operationValues)
        {
            _OperationValues = operationValues;
        }
    }

    public class OperationValues
    {
        /// <summary>
        /// 上传文件最大限制 单位：字节
        /// </summary>
        public int MaxLength { get; set; } = 1024 * 1024 * 4;
    }
}
