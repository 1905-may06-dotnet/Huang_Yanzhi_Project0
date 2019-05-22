using Microsoft.VisualStudio.TestTools.UnitTesting;
using PizzaBoxDomain;
using PizzaBoxData;
using PizzaBoxData.data;
using PizzaBoxClient;

namespace PizzaBoxTesting
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void TestMethod2()
        {
            Client cl = new Client();          
            var actualresult=cl.OnlyCharSpace("HI world");
            var expectedResult = true;
            Assert.AreEqual(expectedResult,actualresult);
        }
    }
}
