using Microsoft.AspNetCore.Http;
using NBeeNET.Mjolnir.Storage.Core;
using NBeeNET.Mjolnir.Storage.Core.Models;
using NBeeNET.Mjolnir.Storage.Job.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBeeNET.Mjolnir.Storage.Job.Service
{
    public class JsonHandleService
    {
        private TempStorageOperation tempStorage;
        public JsonHandleService()
        {
            tempStorage = new TempStorageOperation();
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
            DirectoryInfo tempDirectoryInfo = new DirectoryInfo(tempStorage.BasePath);

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
        /// 设置Job信息
        /// </summary>
        /// <param name="jobModel"></param>
        /// <param name="isDelete"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        public async Task<bool> SetJob(JobInput jobInput)
        {
            bool result = false;
            try
            {
                //判断Json文件是否存在
                var jsonFilePath = Path.Combine(tempStorage.BasePath, jobInput.Id) + "\\" + jobInput.Id + ".json";
                if (!File.Exists(jsonFilePath))
                {
                    return false;
                }

                //设置更新Json文件信息
                JsonFile JsonFileModel = JsonFile.ReadFrom(jsonFilePath);
                var jobInfo = JsonFileModel.Details.Where(t => t.Key == jobInput.Key)?.First();
                jobInfo.State = jobInput.State;
                jobInfo.Value = jobInput.Value;
                await JsonFileModel.SaveAs(tempStorage.GetJsonFilePath(jobInput.Id));

                #region 开始处理任务，保存文件，复制文件夹，删除文件夹

                //保存临时上传的文件
                if (jobInput.Files != null && jobInput.Files.Count > 0)
                {
                    foreach (var file in jobInput.Files)
                    {
                        await tempStorage.Write(file, jobInput.Id);
                    }
                }

                //复制目录
                foreach (var storageService in Register.StorageService)
                {
                    await storageService.CopyDirectory(tempStorage.GetTempPath(jobInput.Id));
                }

                if (jobInput.IsDeleteTemp)
                {
                    //删除临时文件夹
                    tempStorage.Delete(jobInput.Id);
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



    }
}
