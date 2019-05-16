using NBeeNET.Mjolnir.Storage.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Core.Interface
{
    /// <summary>
    /// job接口
    /// </summary>
    public interface IJob
    {

        JsonFileDetail Run(string tempFilePath);
    }
}
