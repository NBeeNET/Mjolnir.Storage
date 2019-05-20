using Microsoft.VisualStudio.TestTools.UnitTesting;
using NBeeNET.Mjolnir.Storage.Office.Common;
using NBeeNET.Mjolnir.Storage.Office.Serivces;
using NBeeNET.Mjolnir.Storage.Print.Common;

namespace NBeeNET.Mjolnir.Storage.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            PrinterHelper.GetPrinterList();
            //PrinterHelper.GetPrinterStatus("Microsoft Print to PDF");
        }
    }
}
