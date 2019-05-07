using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using NBeeNET.Mjolnir.Storage.Core;
using NBeeNET.Mjolnir.Storage.Core.Interface;
using System;
using System.IO;
using System.Threading.Tasks;

namespace NBeeNET.Mjolnir.Storage.AWSS3
{
    public class StorageService : IStorageService
    {

        public string bucketName { get; set; } = "3824a2880e5769dcc0d1c47af6b44a97573b790c511f3caceb158d3abcfd86c5";

        public string keyName { get; set; } = "nbeenetmjolnir";


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
                IAmazonS3 client = new AmazonS3Client(RegionEndpoint.CNNorth1);

                // Get and create the container
                PutBucketRequest request = new PutBucketRequest();
                request.BucketName = bucketName;
                request.BucketRegion = S3Region.CN;    
                await client.PutBucketAsync(request);

                DirectoryInfo directoryInfo = new DirectoryInfo(sourceDir);
                foreach (var file in directoryInfo.GetFiles())
                {
                    var filename = StorageOperation.GetPath() + "/" + file.Name;

                    PutObjectRequest objectRequest = new PutObjectRequest()
                    {
                        FilePath = file.FullName,
                        BucketName = bucketName,
                        Key = keyName
                    };

                    await client.PutObjectAsync(objectRequest);

                }

                result = true;

            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }


        static void CreateABucket()
        {

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
}
