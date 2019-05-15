using Microsoft.VisualStudio.TestTools.UnitTesting;
using NBeeNET.Mjolnir.Storage.Office.Common;

namespace NBeeNET.Mjolnir.Storage.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            PrinterHelper.GetPrinterStatus("Microsoft Print to PDF");
        }
    }
}
