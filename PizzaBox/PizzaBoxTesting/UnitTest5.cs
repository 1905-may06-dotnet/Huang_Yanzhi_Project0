using Microsoft.VisualStudio.TestTools.UnitTesting;
using PizzaBoxDomain;
using PizzaBoxData;
using PizzaBoxData.data;
using PizzaBoxClient;

namespace PizzaBoxTesting
{
    [TestClass]
    public class UnitTest5
    {
        [TestMethod]
        public void TestMethod5()
        {
            PizzaBoxData.data.Item item = new Item() ;
            item.Size = "Large";       
            var actualresult=item.ConvertSizeToNumber();
            var expectedResult = 2;
            Assert.AreEqual(expectedResult,actualresult);
        }
    }
}
