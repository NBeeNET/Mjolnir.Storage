using System;
using System.Collections.Generic;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Core.Common
{
    public class DebugConsole
    {
        public static void WriteLine(string str)
        {
            if (Register.IsDebug)
            {
                System.Console.ForegroundColor = ConsoleColor.DarkYellow;
                System.Console.WriteLine(DateTime.Now + " | " + str);
            }
        }
    }
}
