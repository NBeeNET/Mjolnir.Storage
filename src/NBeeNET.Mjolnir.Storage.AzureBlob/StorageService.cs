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
        public string B { get; set; } = "B1";

        /// <summary>
        /// AzureBlob Container名称
        /// </summary>
        public string Container { get; set; } = "upload";//

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
                var context = new StorageContext();

                // Get and create the container
                var blobContainer = context.BlobClient.GetContainerReference(Container);
                await blobContainer.CreateIfNotExistsAsync();

                DirectoryInfo directoryInfo = new DirectoryInfo(sourceDir);
                foreach (var file in directoryInfo.GetFiles())
                {
                    var filename = StorageOperation.GetPath() + "/" + file.Name;

                    var blob = blobContainer.GetBlockBlobReference(filename);
                    await blob.UploadFromFileAsync(file.FullName);
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

        public StorageContext()
        {
            _storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=get6;AccountKey=dpC3WSz7aUACwWQ8INEndZZmv0K8T9E1uz9N5WPgDB67FGgWrgUZGjnhzzGkV+xTnQ8Zu+4FfW8Rtl8N9FxljA==;EndpointSuffix=core.chinacloudapi.cn");
        }

        public CloudBlobClient BlobClient
        {
            get { return _storageAccount.CreateCloudBlobClient(); }
        }

    }
}
