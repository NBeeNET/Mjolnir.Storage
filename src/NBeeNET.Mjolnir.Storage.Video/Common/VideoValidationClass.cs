using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Video
{
    public class VideoValidationClass
    {
        /// <summary>
        /// 是否为视频
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns>True(是视频)、False(不是视频)</returns>
        public static bool IsCheck(byte[] bytes)
        {
            if (GetVideoFormat(bytes) != VideoFormat.unknown)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 是否为视频
        /// </summary>
        /// <param name="file"></param>
        /// <returns>True(是视频)、False(不是视频)</returns>
        public static bool IsCheck(IFormFile file)
        {
            byte[] fileBytes;
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                fileBytes = ms.ToArray();
            }

            return GetVideoFormat(fileBytes) != VideoFormat.unknown;
        }
        private enum VideoFormat
        {
            ogg,
            mpeg4,
            webm,
            unknown
        }

        private static VideoFormat GetVideoFormat(byte[] bytes)
        {

            return VideoFormat.unknown;
        }
    }
}
