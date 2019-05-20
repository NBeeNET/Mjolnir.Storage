using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NBeeNET.Mjolnir.Storage.Job.Interface
{
    public interface IScheduler
    {
        /// <summary> 
        /// Returns the name of the <see cref="IScheduler" />.
        /// </summary>
        string SchedulerName { get; }

        Task Start();

        void AddJob(IJobExecutionContext jobContext);

        IJobDetail GetJobDetail(string jobKey);

        IEnumerable<string> GetJobKeys(string matcher = "");

        void Clear();
    }
}
