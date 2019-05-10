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
            bmp,
            jpeg,
            gif,
            tiff,
            png,
            unknown
        }

        private static VideoFormat GetVideoFormat(byte[] bytes)
        {
            var bmp = Encoding.ASCII.GetBytes("BM");     // BMP
            var gif = Encoding.ASCII.GetBytes("GIF");    // GIF
            var png = new byte[] { 137, 80, 78, 71 };    // PNG
            var tiff = new byte[] { 73, 73, 42 };         // TIFF
            var tiff2 = new byte[] { 77, 77, 42 };         // TIFF
            var jpeg = new byte[] { 255, 216, 255, 224 }; // jpeg
            var jpeg2 = new byte[] { 255, 216, 255, 225 }; // jpeg canon

            if (bmp.SequenceEqual(bytes.Take(bmp.Length)))
                return VideoFormat.bmp;

            if (gif.SequenceEqual(bytes.Take(gif.Length)))
                return VideoFormat.gif;

            if (png.SequenceEqual(bytes.Take(png.Length)))
                return VideoFormat.png;

            if (tiff.SequenceEqual(bytes.Take(tiff.Length)))
                return VideoFormat.tiff;

            if (tiff2.SequenceEqual(bytes.Take(tiff2.Length)))
                return VideoFormat.tiff;

            if (jpeg.SequenceEqual(bytes.Take(jpeg.Length)))
                return VideoFormat.jpeg;

            if (jpeg2.SequenceEqual(bytes.Take(jpeg2.Length)))
                return VideoFormat.jpeg;

            return VideoFormat.unknown;
        }
    }
}
