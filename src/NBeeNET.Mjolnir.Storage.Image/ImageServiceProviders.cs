using NBeeNET.Mjolnir.Storage.Core.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Image
{

    public static class ImageServiceProviders
    {
        public static List<ServiceProvider> Providers { get; set; } = new List<ServiceProvider>();
    }

    public class ServiceProvider
    {
        public IStorageService Service { get; set; }

        public IStorageOptions Options { get; set; }
    }
}
