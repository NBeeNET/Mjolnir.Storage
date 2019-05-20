using Microsoft.VisualStudio.TestTools.UnitTesting;
using NBeeNET.Mjolnir.Storage.Job;
using NBeeNET.Mjolnir.Storage.Job.Implement;
using NBeeNET.Mjolnir.Storage.Job.Interface;
using System;
using System.Threading;

namespace NBeeNET.Mjolnir.Storage.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {

            //Scheduler scheduler = new Scheduler("Test");

            //var jobContext = JobBuilder.Create<CreateSmallJob>()
            //     .WithName("ceshi")
            //     .AddJobData("tempFilePath", @"C:\Users\liuxiangyu\Pictures\Camera Roll\104.jpg")
            //     .Initialize();

            //scheduler.AddJob(jobContext);


            //scheduler.AddJob(JobBuilder.Create<CreateSmallJob>()
            //  .WithName("ceshi")
            //  .AddJobData("tempFilePath", @"C:\Users\liuxiangyu\Pictures\Camera Roll\104.jpg")
            //  .Initialize());

            //scheduler.Start();

            Console.WriteLine("End:" + DateTime.Now);
          
        }

    }
}
