using NBeeNET.Mjolnir.Storage.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Core.Interface
{
    /// <summary>
    /// Job接口
    /// </summary>
    public interface IJob
    {

        JsonFileValues Run(string tempFilePath, JsonFileValues job);
    }
}
