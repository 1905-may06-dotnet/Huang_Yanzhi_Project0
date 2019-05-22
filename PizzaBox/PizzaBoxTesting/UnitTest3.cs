using Microsoft.VisualStudio.TestTools.UnitTesting;
using PizzaBoxDomain;
using PizzaBoxData;
using PizzaBoxData.data;
using PizzaBoxClient;

namespace PizzaBoxTesting
{
    [TestClass]
    public class UnitTest3
    {
        [TestMethod]
        public void TestMethod3()
        {
            Client cl = new Client();          
            var actualresult=cl.OnlyCharNum("12345HI");
            var expectedResult = true;
            Assert.AreEqual(expectedResult,actualresult);
        }
    }
}
