using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using NBeeNET.Mjolnir.Storage.Core;
using NBeeNET.Mjolnir.Storage.Core.Interface;
using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;

namespace NBeeNET.Mjolnir.Storage.AzureBlob
{
    /// <summary>
    /// AzureBlob 存储
    /// </summary>
    public class StorageService : IStorageService
    {
        public string ConnectionString { get; set; } = "";

        /// <summary>
        /// AzureBlob Container名称
        /// </summary>
        public string Container { get; set; } = "upload";//

        private StorageContext context;

        public StorageService()
        {
            
        }
        /// <summary>
        /// 复制文件夹
        /// </summary>
        /// <param name="sourceDir">源文件夹</param>
        /// <param name="destinationDir">目标文件夹</param>
        /// <param name="isOverwriteExisting">是否覆盖现有</param>
        /// <returns></returns>
        public async Task<bool> CopyDirectory(string sourceDir, string destinationDir, bool isOverwriteExisting)
        {
            bool result = false;
            try
            {
                if (ConnectionString == "")
                {
                    return false;
                }
                //创建上下文
                context = new StorageContext(this.ConnectionString);
                
                // Get and create the container
                var blobContainer = context.BlobClient.GetContainerReference(Container);
                await blobContainer.CreateIfNotExistsAsync();

                DirectoryInfo directoryInfo = new DirectoryInfo(sourceDir);
                foreach (var file in directoryInfo.GetFiles())
                {
                    //上传文件
                    var filename = StorageOperation.GetPath() + "/" + file.Name;

                    var blob = blobContainer.GetBlockBlobReference(filename);
                    await blob.UploadFromFileAsync(file.FullName);

                    Console.WriteLine("Azure Url: http://cnd.get6.cn/upload/" + filename);
                }
                
                result = true;

            }
            catch
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="sourceDir"></param>
        /// <param name="destinationDir"></param>
        /// <returns></returns>
        public bool MoveDirectory(string sourceDir, string destinationDir)
        {
            bool result = false;
            try
            {
                DirectoryInfo destinationDirInfo = new DirectoryInfo(destinationDir);
                if (destinationDirInfo.GetDirectories().Length > 0 || destinationDirInfo.GetFiles().Length > 0)
                {
                    destinationDirInfo.Delete(true);
                    destinationDirInfo.Create();
                }
                Directory.Move(sourceDir, destinationDir);
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public bool DeleteDirectory(string guid)
        {
            bool result = false;
            return result;
        }

        public string GetSavePath()
        {
           return string.Empty;
        }

        public string GetPath()
        {
            return string.Empty;
        }

        public string GetUrl(string filename)
        {
            return string.Empty;
        }
    }


    public class StorageContext
    {
        private CloudStorageAccount _storageAccount;

        public StorageContext(string connectionString)
        {
            _storageAccount = CloudStorageAccount.Parse(connectionString);
        }

        public CloudBlobClient BlobClient
        {
            get { return _storageAccount.CreateCloudBlobClient(); }
        }

    }
}
