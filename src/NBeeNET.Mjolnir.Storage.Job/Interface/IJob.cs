using System.Threading.Tasks;

namespace NBeeNET.Mjolnir.Storage.Job.Interface
{
    /// <summary>
    /// 作业接口
    /// </summary>
    public interface IJob
    {

        string Key { get; set; }

        /// <summary>
        /// 执行作业
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task Execute(IJobExecutionContext context);
    }
}
