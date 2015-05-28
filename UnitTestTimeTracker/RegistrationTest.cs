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

            string expected = "/Pages/MainPivotPage.xaml?projectName=a&projectId=b&finalDate=c";
            var factory = new UriFactory();
            string uri = factory.CreateDataUri("a", "b", "c");
            Assert.AreEqual(expected, uri);

        }
    }
}
