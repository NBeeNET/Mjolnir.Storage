﻿using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using NBeeNET.Mjolnir.Storage.Core;
using NBeeNET.Mjolnir.Storage.Core.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace NBeeNET.Mjolnir.Storage.AWSS3
{
    public class StorageService : IStorageService
    {

        public string AwsAccessKeyId { get; set; } = "";

        public string AwsSecretAccessKey { get; set; } = "";

        public string BucketName { get; set; } = "";

        public static IAmazonS3 client;
        /// <summary>
        /// 复制文件夹
        /// </summary>
        /// <param name="sourceDir">源文件夹</param>
        /// <returns></returns>
        public async Task<bool> CopyDirectory(string sourceDir)
        {
            bool result = false;
            try
            {
                if (string.IsNullOrEmpty(AwsAccessKeyId)  || string.IsNullOrEmpty(AwsSecretAccessKey) || string.IsNullOrEmpty(BucketName))
                {
                    return result;
                }
                client = new AmazonS3Client(AwsAccessKeyId, AwsSecretAccessKey, RegionEndpoint.USEast1);

                //验证名称为bucketName的bucket是否存在，不存在则创建  
                if (!await checkBucketExists(BucketName))
                {
                    // Get and create the container
                    PutBucketRequest request = new PutBucketRequest();
                    request.BucketName = BucketName;
                    request.BucketRegion = S3Region.US;
                    await client.PutBucketAsync(request);
                }

                DirectoryInfo directoryInfo = new DirectoryInfo(sourceDir);
                foreach (var file in directoryInfo.GetFiles())
                {
                    //上传文件  
                    var filename = StorageOperation.GetPath() + "/" + file.Name;
                    PutObjectRequest objectRequest = new PutObjectRequest()
                    {
                        FilePath = file.FullName,
                        BucketName = BucketName,
                        Key = filename
                    };

                    var response = await client.PutObjectAsync(objectRequest);
                    var url= client.GeneratePreSignedURL(BucketName, filename, new DateTime(2020, 12, 31), null);
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("AWS Url:" + url);
                }

                result = true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                result = false;
            }
            return result;
        }


        public async Task<bool> checkBucketExists(String bucketName)
        {
            ListBucketsResponse response = await client.ListBucketsAsync();
            foreach (S3Bucket bucket in response.Buckets)
            {
                if (bucket.BucketName == bucketName)
                {
                    return true;
                }
            }
            return false;
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
