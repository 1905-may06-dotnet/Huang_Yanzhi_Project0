using Microsoft.VisualStudio.TestTools.UnitTesting;
using PizzaBoxDomain;
using PizzaBoxData;
using PizzaBoxData.data;
using PizzaBoxClient;

namespace PizzaBoxTesting
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Client cl = new Client();          
            var actualresult=cl.OnlyNum("12345HI");
            var expectedResult = false;
            Assert.AreEqual(expectedResult,actualresult);
        }
    }
}
