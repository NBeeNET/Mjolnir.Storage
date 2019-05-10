using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Core.Common
{
    /// <summary>
    /// 控制台输出帮助
    /// </summary>
    public static class ConsoleHelper
    {

        /// <summary>
        /// 输出Info
        /// </summary>
        /// <param name="text"></param>
        public static void Info(string msg)
        {
          
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(string.Format("{0}{1}{2}", DateTime.Now, "Info", msg));
        }

        /// <summary>
        /// 告警
        /// </summary>
        /// <param name="msg"></param>
        public static void Warn(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(string.Format("{0}{1}{2}", DateTime.Now, "Warn", msg));
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="msg"></param>
        public static void Error(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(string.Format("{0}{1}{2}", DateTime.Now, "Error", msg));
        }
    }
}
