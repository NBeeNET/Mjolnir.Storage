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
            wav,
            mp3,
            ogg,
            acc,
            webm,
            unknown
        }

        private static AudioFormat GetAudioFormat(byte[] bytes)
        {


            return AudioFormat.unknown;
        }
    }
}
