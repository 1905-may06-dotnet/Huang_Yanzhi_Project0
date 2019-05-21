using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaBoxDomain
{
    public class User
    {
        private int uid = 0;
        private string userName = "";
        private string password = "";
        private string name = "";
        public User(string un, string pw, string name)
        {
            this.userName = un;
            this.password = pw;
            this.name = name;
        }
        public void displayUseriInfo()
        {
            Console.WriteLine($"User ID: {uid}, Name: {name}");
        }

    }
}
