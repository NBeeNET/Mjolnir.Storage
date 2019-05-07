using System;
using System.Collections.Generic;
using System.Text;

namespace NBeeNET.Mjolnir.Storage
{
    public class Register
    {
        public static List<Core.Interface.IStorageService> _IStorageService = new List<Core.Interface.IStorageService>();
        public static void AddStorage(Core.Interface.IStorageService storageService)
        {
            _IStorageService.Add(storageService);
        }

    }
}
