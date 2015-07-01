using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeTracker;

namespace UnitTestTimeTracker
{
    [TestClass]
    public class RegistrationTest
    {
        [TestMethod]
        public void TestCheckWorkingTime()
        {

        }

        [TestMethod]
        public void CreateDataUriTest()
        {
            string expected = "/Pages/MainPivotPage.xaml?projectName=a&projectId=b&finalDate=c&latitude=d&longitude=e";
            var factory = new UriFactory();
            string uri = factory.CreateDataUri("a", "b", "c", "d", "e");
            Assert.AreEqual(expected, uri);

        }
    }
}
