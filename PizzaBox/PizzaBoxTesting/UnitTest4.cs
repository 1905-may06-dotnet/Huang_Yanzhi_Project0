using Microsoft.VisualStudio.TestTools.UnitTesting;
using PizzaBoxDomain;
using PizzaBoxData;
using PizzaBoxData.data;
using PizzaBoxClient;

namespace PizzaBoxTesting
{
    [TestClass]
    public class UnitTest4
    {
        [TestMethod]
        public void TestMethod4()
        {
            PizzaBoxDomain.PizzaOrder po = new PizzaBoxDomain.PizzaOrder();          
            var actualresult=po.calculateItemPrice("Large",5);
            var expectedResult = 13;
            Assert.AreEqual(expectedResult,actualresult);
        }
    }
}
