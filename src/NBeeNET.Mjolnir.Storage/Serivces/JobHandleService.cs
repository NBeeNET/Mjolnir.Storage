using NBeeNET.Mjolnir.Storage.Core;
using NBeeNET.Mjolnir.Storage.Core.Common;
using NBeeNET.Mjolnir.Storage.Core.Implement;
using NBeeNET.Mjolnir.Storage.Core.Interface;
using NBeeNET.Mjolnir.Storage.Core.Models;
using NBeeNET.Mjolnir.Storage.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NBeeNET.Mjolnir.Storage.Service
{
    public class JobHandleService
    {
     
        public JobHandleService()
        {
          
        }
        /// <summary>
        /// 获取Job信息
        /// </summary>
        /// <param name="id">Guid</param>
        /// <param name="key">Job任务类型</param>
        /// <param name="state">Job状态</param>
        /// <returns></returns>
        public List<JobOutput> GetJobs(string id = "", string key = "", string state = "")
        {
            List<JobOutput> jobModelList = new List<JobOutput>();
            DirectoryInfo tempDirectoryInfo = new DirectoryInfo(TempStorageOperation.BasePath);

            try
            {
                //根据id查询
                if (!string.IsNullOrEmpty(id))
                {
                    var searchDirectorys = tempDirectoryInfo.GetDirectories(id, SearchOption.TopDirectoryOnly);
                    if (searchDirectorys.Length > 0)
                    {
                        var idDirectory = searchDirectorys[0];
                        var searchFiles = idDirectory.GetFiles(idDirectory.Name + ".json");
                        if (searchFiles.Length > 0)
                        {
                            var jsonFileInfo = searchFiles[0];
                            var JsonFileModel = JsonFile.ReadFrom(jsonFileInfo.FullName);
                            if (JsonFileModel.Details.Count > 0)
                            {
                                var jobModels = JobOutput.GetJobModels(JsonFileModel);
                                jobModelList.AddRange(jobModels);
                            }
                        }
                    }
                }
                else //遍历所有目录获取json文件
                {
                    foreach (DirectoryInfo dir in tempDirectoryInfo.GetDirectories())
                    {
                        var searchFiles = dir.GetFiles(dir.Name + ".json");
                        if (searchFiles.Length > 0)
                        {
                            var jsonFileInfo = searchFiles[0];
                            var JsonFileModel = JsonFile.ReadFrom(jsonFileInfo.FullName);
                            if (JsonFileModel.Details.Count > 0)
                            {
                                var jobModels = JobOutput.GetJobModels(JsonFileModel);
                                jobModelList.AddRange(jobModels);
                            }
                        }
                    }
                }

                if (jobModelList.Count > 0)
                {
                    //根据key查询
                    if (!string.IsNullOrEmpty(key))
                    {
                        jobModelList = jobModelList.Where(t => t.Key == key).ToList();
                    }

                    // //根据state查询
                    if (!string.IsNullOrEmpty(state))
                    {
                        jobModelList = jobModelList.Where(t => t.State == state).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return jobModelList;
        }

        /// <summary>
        /// 添加Jobs信息
        /// </summary>
        /// <param name="jobInputs"></param>
        /// <returns></returns>
        public async Task<bool> AddJobs(List<JobInput> jobInputs)
        {
            bool result = false;
            try
            {
                if (jobInputs.Count > 0)
                {
                    for (int i = 0; i < jobInputs.Count; i++)
                    {
                        var jobInput = jobInputs[i];

                        //判断Json文件是否存在
                        var jsonFilePath = Path.Combine(TempStorageOperation.BasePath, jobInput.Id) + "\\" + jobInput.Id + ".json";
                        if (!File.Exists(jsonFilePath))
                        {
                            continue;
                        }

                        //获取Json对象
                        JsonFile JsonFileModel = JsonFile.ReadFrom(jsonFilePath);
                        if (JsonFileModel.Details != null)
                        {
                            if (JsonFileModel.Details.Where(t => t.Key == jobInput.Key).Count() == 0)
                            {
                                JsonFileDetail jobInfo = new JsonFileDetail();
                                jobInfo.Key = jobInput.Key;
                                jobInfo.Param = jobInput.Param;
                                jobInfo.State = jobInput.State;
                                jobInfo.Value = jobInput.Value;
                                JsonFileModel.Details.Add(jobInfo);
                            }
                            else
                            {
                                var jobInfo = JsonFileModel.Details.Where(t => t.Key == jobInput.Key)?.First();
                                jobInfo.Key = jobInput.Key;
                                jobInfo.Param = jobInput.Param;
                                jobInfo.State = jobInput.State;
                                jobInfo.Value = jobInput.Value;
                            }
                        }
                        else
                        {
                            JsonFileModel.Details = new List<JsonFileDetail>();
                            JsonFileDetail jobInfo = new JsonFileDetail();
                            jobInfo.Key = jobInput.Key;
                            jobInfo.Param = jobInput.Param;
                            jobInfo.State = jobInput.State;
                            jobInfo.Value = jobInput.Value;
                            JsonFileModel.Details.Add(jobInfo);
                        }

                        await JsonFileModel.SaveAs(TempStorageOperation.GetJsonFilePath(jobInput.Id));

                        #region 开始处理任务--复制文件夹，删除临时文件夹

                        //复制目录
                        foreach (var storageService in Register.StorageService)
                        {
                            await storageService.CopyDirectory(TempStorageOperation.GetTempPath(jobInput.Id));
                        }

                        if (jobInput.IsDeleteTemp)
                        {
                            //删除临时文件夹
                            TempStorageOperation.Delete(jobInput.Id);
                        }
                        #endregion
                    }

                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// 修改Job信息
        /// </summary>
        /// <param name="jobModel"></param>
        /// <param name="isDelete"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        public async Task<bool> ModifyJob(JobInput jobInput)
        {
            bool result = false;
            try
            {
                //判断Json文件是否存在
                var jsonFilePath = Path.Combine(TempStorageOperation.BasePath, jobInput.Id) + "\\" + jobInput.Id + ".json";
                if (!File.Exists(jsonFilePath))
                {
                    return false;
                }

                //设置更新Json文件信息
                JsonFile JsonFileModel = JsonFile.ReadFrom(jsonFilePath);
                var jobInfo = JsonFileModel.Details.Where(t => t.Key == jobInput.Key)?.First();
                jobInfo.State = jobInput.State;
                jobInfo.Value = jobInput.Value;
                await JsonFileModel.SaveAs(TempStorageOperation.GetJsonFilePath(jobInput.Id));

                #region 开始处理任务，保存文件，复制文件夹，删除文件夹

                //保存临时上传的文件
                if (jobInput.Files != null && jobInput.Files.Count > 0)
                {
                    foreach (var file in jobInput.Files)
                    {
                        await TempStorageOperation.Write(file, jobInput.Id);
                    }
                }

                //复制目录
                foreach (var storageService in Register.StorageService)
                {
                    await storageService.CopyDirectory(TempStorageOperation.GetTempPath(jobInput.Id));
                }

                if (jobInput.IsDeleteTemp)
                {
                    //删除临时文件夹
                    TempStorageOperation.Delete(jobInput.Id);
                }
                #endregion
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }


        /// <summary>
        /// 执行处理任务（执行多个Id下的某个任务）
        /// </summary>
        /// <returns></returns>
        public async Task RunJobs(List<JobInput> jobInputs)
        {
            StorageOperation storage = new StorageOperation();
        
            if (jobInputs.Count > 0)
            {
                Scheduler scheduler = new Scheduler("UploadHandleService");

                DebugConsole.WriteLine(" 开始任务处理...");
                for (int i = 0; i < jobInputs.Count; i++)
                {
                    var jobInput = jobInputs[i];

                    //判断Json文件是否存在
                    var tempJsonPath = Path.Combine(TempStorageOperation.BasePath, jobInput.Id) + "\\" + jobInput.Id + ".json";
                    if (!File.Exists(tempJsonPath))
                    {
                        continue;
                    }

                    //获取Json对象
                    JsonFile JsonFileModel = JsonFile.ReadFrom(tempJsonPath);
                    //临时文件全路径
                    var tempFilePath = Path.Combine(TempStorageOperation.BasePath, jobInput.Id) + "\\" + JsonFileModel.FileName;

                    //获取要执行 job
                    JsonFileDetail job = JsonFileModel.Details.Where(t => t.Key == jobInput.Key)?.First();
                    if (job != null && job.State == "0")//等于0可执行
                    {
                        //判断Job类型是否有
                        IJob outJob = null;
                        var ret = Register.TryGetJob(job.Key, out outJob);
                        if (ret)
                        {
                            //添加Job
                            var jobContext = JobBuilder.Create(outJob.GetType())
                           .WithName(job.Key)
                           .AddJobData("tempFilePath", tempFilePath)
                           .AddJobData("tempJsonPath", tempJsonPath)
                           .Initialize();
                            scheduler.AddJob(jobContext);

                        }
                    }

                }

                //开始处理任务
                await scheduler.Start();


            }
            DebugConsole.WriteLine(" 结束任务处理...");

        }

        /// <summary>
        ///  执行处理任务（执行多个Id下的所有任务）
        /// </summary>
        /// <param name="jobIdInputs"></param>
        public async Task RunJobs(List<string> jobIdInputs)
        {
            StorageOperation storage = new StorageOperation();
            TempStorageOperation tempStorage = new TempStorageOperation();
            if (jobIdInputs.Count > 0)
            {
                Scheduler scheduler = new Scheduler("UploadHandleService");

                DebugConsole.WriteLine(" 开始任务处理...");
                for (int i = 0; i < jobIdInputs.Count; i++)
                {
                    var jobId = jobIdInputs[i];

                    //判断Json文件是否存在
                    var jsonFilePath = Path.Combine(TempStorageOperation.BasePath, jobId) + "\\" + jobId + ".json";
                    if (!File.Exists(jsonFilePath))
                    {
                        continue;
                    }

                    //获取Json对象
                    JsonFile JsonFileModel = JsonFile.ReadFrom(jsonFilePath);
                    //临时文件全路径
                    var tempFilePath = Path.Combine(TempStorageOperation.BasePath, jobId) + "\\" + JsonFileModel.FileName;

                    //获取要执行Id下的所有job

                    if (JsonFileModel.Details != null && JsonFileModel.Details.Count > 0)
                    {
                        foreach (var job in JsonFileModel.Details)
                        {
                            if (job.State == "0")
                            {
                                IJob outJob = null;
                                //判断Job类型是存在，存在则添加
                                if (Register.TryGetJob(job.Key, out outJob))
                                {
                                    //添加Job
                                    var jobContext = JobBuilder.Create(outJob.GetType())
                                   .WithName(job.Key)
                                   .AddJobData("tempFilePath", tempFilePath)
                                   .Initialize();
                                    scheduler.AddJob(jobContext);

                                }
                            }
                        }
                    }

                }

                //开始处理任务
                await scheduler.Start();


            }
            DebugConsole.WriteLine(" 结束任务处理...");
        }
    }
}
