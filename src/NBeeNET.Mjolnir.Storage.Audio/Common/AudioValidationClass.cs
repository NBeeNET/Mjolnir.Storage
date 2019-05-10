using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Audio
{
    public class AudioValidationClass
    {
        /// <summary>
        /// 是否为音频
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns>True(是音频)、False(不是音频)</returns>
        public static bool IsCheck(byte[] bytes)
        {
            if (GetAudioFormat(bytes) != AudioFormat.unknown)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 是否为音频
        /// </summary>
        /// <param name="file"></param>
        /// <returns>True(是音频)、False(不是音频)</returns>
        public static bool IsCheck(IFormFile file)
        {
            byte[] fileBytes;
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                fileBytes = ms.ToArray();
            }

            return GetAudioFormat(fileBytes) != AudioFormat.unknown;
        }
        private enum AudioFormat
        {
            bmp,
            jpeg,
            gif,
            tiff,
            png,
            unknown
        }

        private static AudioFormat GetAudioFormat(byte[] bytes)
        {
            var bmp = Encoding.ASCII.GetBytes("BM");     // BMP
            var gif = Encoding.ASCII.GetBytes("GIF");    // GIF
            var png = new byte[] { 137, 80, 78, 71 };    // PNG
            var tiff = new byte[] { 73, 73, 42 };         // TIFF
            var tiff2 = new byte[] { 77, 77, 42 };         // TIFF
            var jpeg = new byte[] { 255, 216, 255, 224 }; // jpeg
            var jpeg2 = new byte[] { 255, 216, 255, 225 }; // jpeg canon

            if (bmp.SequenceEqual(bytes.Take(bmp.Length)))
                return AudioFormat.bmp;

            if (gif.SequenceEqual(bytes.Take(gif.Length)))
                return AudioFormat.gif;

            if (png.SequenceEqual(bytes.Take(png.Length)))
                return AudioFormat.png;

            if (tiff.SequenceEqual(bytes.Take(tiff.Length)))
                return AudioFormat.tiff;

            if (tiff2.SequenceEqual(bytes.Take(tiff2.Length)))
                return AudioFormat.tiff;

            if (jpeg.SequenceEqual(bytes.Take(jpeg.Length)))
                return AudioFormat.jpeg;

            if (jpeg2.SequenceEqual(bytes.Take(jpeg2.Length)))
                return AudioFormat.jpeg;

            return AudioFormat.unknown;
        }
    }
}
