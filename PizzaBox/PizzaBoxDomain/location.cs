using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaBoxDomain
{
    public class Location
    {
        int LocationID = 0;
        string address, city, state, zipcode;
        public Location(int LID, string add, string c, string s, string z)
        {
            LocationID = LID;
            address = add;
            city = c;
            state = s;
            zipcode=z;
        }
        public void displayDetails()
        {
            Console.WriteLine($"Location {LocationID}");
            Console.WriteLine(address+" "+ city + " " + state +" " + zipcode);
        }

    }

}
