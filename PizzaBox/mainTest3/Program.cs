using System;
using PizzaBoxDomain;
using PizzaBoxData;
using PizzaBoxData.data;
using PizzaBoxClient;
using System.Linq;

namespace mainTest3
{
    class Program
    {
        static void Main(string[] args)
        {

             Client cl = new PizzaBoxClient.Client();
            cl.RunApplication();

        }
    }
}
