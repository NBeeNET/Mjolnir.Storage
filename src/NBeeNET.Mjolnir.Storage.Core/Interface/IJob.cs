using NBeeNET.Mjolnir.Storage.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NBeeNET.Mjolnir.Storage.Core.Interface
{
    /// <summary>
    /// Job接口
    /// </summary>
    public interface IJob
    {

        Task<JsonFileValues> Run(string tempFilePath, JsonFileValues job);
    }
}
