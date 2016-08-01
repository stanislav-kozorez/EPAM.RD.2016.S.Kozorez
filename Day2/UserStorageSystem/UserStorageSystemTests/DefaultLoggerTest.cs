using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserStorageSystem;

namespace UserStorageSystemTests
{
    [TestClass]
    public class DefaultLoggerTest
    {
        [TestMethod]
        public void Log_CreatesLogFile()
        {
            var logger = new DefaultLogger(); 
            logger.Log(System.Diagnostics.TraceEventType.Information, "Test");
            Assert.IsTrue(File.Exists("AppLog.log"));          
        }
    }
}
