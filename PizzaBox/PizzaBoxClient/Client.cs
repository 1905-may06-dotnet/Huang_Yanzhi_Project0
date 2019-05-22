using System;
using System.Collections.Generic;
using System.Text;
using PizzaBoxDomain;
using PizzaBoxData;
using PizzaBoxData.data;
using System.Linq;

namespace PizzaBoxClient
{
    public class Client
    {
        private const int MainMenu = 0, SignUpPage = 1, SignInPage = 2, Admin = 3, OrderPizzaPage = 1, viewOrderHistoryPage = 2, signOut = 3, UserMenuPage=0;
        private int menuSelect = 0, mode=0, adminSelection=0;
        string decision = "", BackToMainMenu = "q"; 
        public int selectedLocaitonID = 0, LoginUserID=0;
        public string[] toppingType = new string[8] { "Pepperoni", "Mushrooms", "Onions", "Sausage", "Bacon", "Extra cheese", "Black olives", "Green peppers" };
        public void RunApplication()
        {           
            do
            {
                SelectUserType();
                decision = "";
                if (menuSelect == Admin)
                {
                    string str = "";
                    Console.WriteLine("Please enter password: ");
                    do
                    {
                        str = Console.ReadLine();
                        if (str != "pw"&& str != BackToMainMenu)
                        { Console.WriteLine("Incorrect password, please try again."); }
                    }
                    while (str!= BackToMainMenu&&str!="pw");
                    if (str != BackToMainMenu)
                    { AdminEvents(); }
                }
                else
                { 
                    if (menuSelect == SignUpPage)
                    {
                        Register();
                    }
                    else if (menuSelect == SignInPage)
                    {
                        SignIn();
                    }
                    if (decision != BackToMainMenu)
                    {
                        do
                        {
                            UserEvents();
                        }
                        while (mode != signOut);
                    }
                }
            } while (true);
        }
        public void UserEvents()
        {
            DisplayUserMenuOpstion();
            mode = SelectUserMenuOpstion();
            Crud c = new Crud();          
            if (mode == OrderPizzaPage)
            {
                if (c.UserHasOrder(LoginUserID))
                {                    
                    DateTime now = DateTime.Now;
                    DateTime compareTime = c.getUserOrderLastOrderTime(LoginUserID).AddHours(2);
                    if (now < compareTime)
                    {
                        Console.WriteLine("Only one order is allowed within a 2 hour period.");
                        Console.WriteLine($"You can place your next order in (HH:MM): {(compareTime.Subtract(now)).ToString(@"hh\:mm")}");
                        Console.WriteLine("press Enter key to continue...");
                        string str = Console.ReadLine();
                    }
                    else
                    {
                        OrderingPizza();
                    }
                }
                else
                {
                    do
                    {
                        OrderingPizza();
                    }
                    while (mode != UserMenuPage);
                }
            }
            else if (mode == viewOrderHistoryPage)
            {
                if (c.UserHasOrder(LoginUserID))
                {
                    ViewOrderHistory(LoginUserID);
                    Console.WriteLine("Enter Order ID to view order details or enter (0) to go back to user menu.");
                    int tempSelect = inputValidation(0,999999);
                    if (tempSelect!=0)
                    {
                        UserViewOrderDetails(LoginUserID, tempSelect);
                        Console.WriteLine("press Enter key to continue...");
                        string str = Console.ReadLine();
                    }                   
                }
                else
                {
                    Console.WriteLine("You don't have any Order yet.");
                    Console.WriteLine("press Enter key to continue...");
                    string str = Console.ReadLine();
                }  

            }
            else if (mode == signOut)
            {
                Console.WriteLine("You have signed out of your account.");
                Console.WriteLine("-----------------------------------------");
            }
        }
        public void SelectUserType()
        {
            Console.WriteLine("===============Pizza BOX=================");
            Console.WriteLine("---------------Main Menu-----------------");
            Console.WriteLine("- (1)New User (2)Existing User (3)Admin -");
            Console.WriteLine("-----------------------------------------");
            menuSelect = inputValidation(1, 3);
        }
        public void AdminEvents()
        {
            do
            {
                Console.WriteLine("---------------Admin---------------");
                ViewLocation();
                SelectLocation();
                do
                {
                    DisplayAdminOptions();
                    AdminOptions();
                }
                while (adminSelection != 5&& adminSelection != 6);
            }
            while (adminSelection != 6);
        }
        public void DisplayAdminOptions()
        {
            Console.WriteLine("------------Admin Options----------");
            Console.WriteLine($"------------Location: {selectedLocaitonID}------------");
            Console.WriteLine("(1) View Orders");
            Console.WriteLine("(2) View Sales");
            Console.WriteLine("(3) View Inventory");
            Console.WriteLine("(4) View Customers");
            Console.WriteLine("(5) Select a Location");
            Console.WriteLine("(6) Go Back to Main menu");
            Console.WriteLine("-----------------------------------");
        }
        public void AdminOptions()
        {
            adminSelection = inputValidation(1, 6);
            switch (adminSelection)
            {
                case 1:
                    AdminViewOrders();
                    break;
                case 2:
                    AdminViewSales();
                    break;
                case 3:
                    AdminViewInventory();
                    break;
                case 4:
                    AdminViewCustomers();
                    break;
                default:
                    break;
            }
            if (adminSelection != 5 && adminSelection != 6 && adminSelection!= 1)
            {
                Console.WriteLine("press Enter key to continue...");
                string str = Console.ReadLine();
            }

        }
        public void AdminViewOrders()
        {
            Console.WriteLine("-----------------------------Order History----------------------------------");
            Crud c = new Crud();
            foreach (PizzaBoxData.data.PizzaOrder po in c.getLocationOrderHistory(selectedLocaitonID))
            {
                po.DisplayOrderHistoryForAdmin();
            }
            Console.WriteLine("----------------------------------------------------------------------------");
            Console.WriteLine("Enter Order ID to view order details or enter (0) to go back to Admin menu.");
                int tempSelect = inputValidation(0, 999999);
                if (tempSelect != 0)
                {
                    AdminViewOrderDetails(tempSelect);
                    Console.WriteLine("press Enter key to continue...");
                    string str = Console.ReadLine();
                }
        }
        public void AdminViewSales()
        {
            Crud c = new Crud();
            Console.WriteLine("-----------------------------Sales----------------------------------");
            decimal total = 0.00M;
            foreach (PizzaBoxData.data.PizzaOrder po in c.getLocationOrderHistory(selectedLocaitonID))
            {
                total=total+(decimal)po.Total;
            }
            Console.WriteLine($"Sales: ${total}, as current time: {DateTime.Now.ToString()} ");
            Console.WriteLine("---------------------------------------------------------------------");
        }
        public void AdminViewInventory()
        {
            double doughStored = 5000;
            double doughUsed = 0;
            Crud c = new Crud();
            foreach (PizzaBoxData.data.PizzaOrder po in c.getLocationOrderHistory(selectedLocaitonID))
            {
                foreach (PizzaBoxData.data.Item i in c.GetItemByOrderID(po.OrderId))
                { doughUsed += i.ConvertSizeToNumber()*i.NumberOfPizza; }
            }
            Console.WriteLine("----------------------Inventory---------------------");
            Console.WriteLine($"Dough:         {doughStored-doughUsed}");
            Console.WriteLine($"{toppingType[0]}:     {3000 - getUsedCountOfTopping(toppingType[0])}");
            Console.WriteLine($"{toppingType[1]}:     {3000 - getUsedCountOfTopping(toppingType[1])}");
            Console.WriteLine($"{toppingType[2]}:        {3000 - getUsedCountOfTopping(toppingType[2])}");
            Console.WriteLine($"{toppingType[3]}:       {3000 - getUsedCountOfTopping(toppingType[3])}");
            Console.WriteLine($"{toppingType[4]}:         {3000 - getUsedCountOfTopping(toppingType[4])}");
            Console.WriteLine($"{toppingType[5]}:  {3000 - getUsedCountOfTopping(toppingType[5])}");
            Console.WriteLine($"{toppingType[6]}:  {3000 - getUsedCountOfTopping(toppingType[6])}");
            Console.WriteLine($"{toppingType[7]}: {3000 - getUsedCountOfTopping(toppingType[7])}");
            Console.WriteLine("----------------------------------------------------");
        }
        public int getUsedCountOfTopping(string keyword)
        {
            int count = 0;
            Crud c = new Crud();
            foreach (PizzaBoxData.data.PizzaOrder po in c.getLocationOrderHistory(selectedLocaitonID))
            {
                foreach (PizzaBoxData.data.Item item in c.GetItemByOrderID(po.OrderId))
                {
                    foreach (string t in item.splittedToppings())
                    { if (t == keyword) count += item.NumberOfPizza;  }
                }
            }                       
            return count;
        }
        public void AdminViewCustomers()
        {
            Crud c = new Crud();
            List <string> temp = new List<string>();
            Console.WriteLine("-----------------------------Customers--------------------------------");
            foreach (PizzaBoxData.data.PizzaOrder po in c.getLocationOrderHistory(selectedLocaitonID))
            {
                temp.Add(c.GetUsers((int)po.UserId).DetailForOrderHistory());
            }
            IEnumerable<string> temp2 =temp.Distinct();
            foreach (string u in temp2)
            {
                Console.WriteLine(u); 
            }
            Console.WriteLine("----------------------------------------------------------------------");
        }
        public void Register()
        {
            Crud c = new Crud();
            Console.WriteLine("------------Sign Up Page------------");
            Console.WriteLine("*Enter 'q' to go back to main menu*");
            Console.WriteLine("Please Enter your user name: (only letters and numbers. length: 5-15)");
            string un = "", pw="", fn="", pn="";
            bool exist=false;
            do
            {
                un = inputStringValidation(5,15,1);
                if (decision!= BackToMainMenu)
                {
                    exist = c.UsernameExist(un);
                    if (exist)
                    {
                        Console.WriteLine("Username already exists, please try again.");
                    }
                }
            }
            while (exist&& decision != BackToMainMenu);
            if (decision != BackToMainMenu)
            {
                Console.WriteLine("Please Enter your password: (length: 5-15)");
                pw = inputStringValidation(5, 15, 0);
                if (decision != BackToMainMenu)
                {
                    Console.WriteLine("Please Enter your Name: (only letters and space. length: 2-30)");
                    fn = inputStringValidation(2, 30, 2);
                    if (decision != BackToMainMenu)
                    {
                        Console.WriteLine("Please Enter your phone number: (only numbers. length: 8-15)");
                        pn = inputStringValidation(8, 15, 3);
                        if (decision != BackToMainMenu)
                        {
                            AppUser newUser = new AppUser(un, pw, fn, pn);
                            c.AddUser(newUser);
                            Console.WriteLine("");
                            Console.WriteLine($"Welcome! {fn}");
                            LoginUserID = c.getUserId(un);
                        }
                    }
                }
            }
            
        }
        public void SignIn()
        {
            Console.WriteLine("------------Log In Page------------");
            Console.WriteLine("*Enter 'q' to go back to main menu*");
            Console.WriteLine("Please Enter your user name:");
            Crud c = new Crud();
            string un = "", pw = "";
            bool UNExist = false;
            bool PWMatched = false;
            do
            {
                un = inputStringValidation(5, 15, 1);
                if (decision != BackToMainMenu)
                {
                    UNExist = c.UsernameExist(un);
                    if (!UNExist)
                    {
                        Console.WriteLine("Username does not exist, please try again.");
                    }
                }
            }
            while (!UNExist && decision != BackToMainMenu);
            if (decision != BackToMainMenu)
            {
                Console.WriteLine("Please Enter your password:");
                do
                {
                    pw = inputStringValidation(5, 15, 0);
                    if (decision != BackToMainMenu)
                    {
                        PWMatched = c.PasswordMatched(un, pw);
                        if (!PWMatched)
                        {
                            Console.WriteLine("Incorrect password, please try again.");
                        }
                    }
                }
                while (!PWMatched && decision != BackToMainMenu);
                if (decision != BackToMainMenu)
                {
                    Console.WriteLine("");
                    Console.WriteLine($"Welcome! {c.getUserFullName(un)}");
                    LoginUserID = c.getUserId(un);
                }
            }
        }
        public void DisplayUserMenuOpstion()
        {
            Console.WriteLine("---------User Menu----------");
            Console.WriteLine("(1) Order Pizza");
            Console.WriteLine("(2) View Order History");
            Console.WriteLine("(3) Sign Out");
            Console.WriteLine("----------------------------");
        }
        public int SelectUserMenuOpstion()
        {
            return inputValidation(1,3);
        }
        public void ViewLocation()
        {
            Crud c = new Crud();
            Console.WriteLine("Select A Location Number: ");
            c.DisplayAllLocations();
        }
        public void SelectLocation()
        {
            selectedLocaitonID = inputValidation(1, 3);
            Console.WriteLine($"You Selected Location {selectedLocaitonID}");
        }
        public void OrderingPizza()
        {           
            int selection = 0;
            Crud c = new Crud();
            ViewLocation();
            SelectLocation();
            if (c.UserHasOrder(LoginUserID))
            {
                DateTime now = DateTime.Now;
                DateTime compareTime = c.getUserOrderLastOrderTime(LoginUserID).AddHours(24);
                bool restriction24hrOneLocation = now < compareTime && selectedLocaitonID != c.getUserOrderLastOrderLocationID(LoginUserID);
                if (restriction24hrOneLocation)
                {
                    selectedLocaitonID = c.getUserOrderLastOrderLocationID(LoginUserID);
                    Console.WriteLine("You can only order from 1 location/day.");
                    Console.WriteLine($"You can place your order in other locations in (HH:MM): {(compareTime.Subtract(now)).ToString(@"hh\:mm")}");
                    Console.WriteLine($"Location {c.getUserOrderLastOrderLocationID(LoginUserID)} where you placed your last order is automatically selected.");
                    Console.WriteLine("press Enter key to continue...");
                    string str = Console.ReadLine();
                }
            }
            PizzaBoxDomain.PizzaOrder newOrder = new PizzaBoxDomain.PizzaOrder();
            newOrder.createOrder();
            Console.WriteLine("Enter(1) to confirm order or Enter (2) to cancel order");
            selection = inputValidation(1, 2);
            if (selection == 1)
            {
                StoreOrderInfo(newOrder);
            }
            else if (selection == 2)
            {
                Console.WriteLine("Order was canceled. Press Enter key to continue...");         
                string str = Console.ReadLine();
                mode = UserMenuPage;
            }
        }
        public void StoreOrderInfo(PizzaBoxDomain.PizzaOrder o)
        {
            Crud c = new Crud();
            PizzaBoxData.data.PizzaOrder pOrder = new PizzaBoxData.data.PizzaOrder(DateTime.Now.ToString("MM/dd/yyyy HH:mm"), (double)o.total, LoginUserID, selectedLocaitonID);
            c.AddOrder(pOrder);
            foreach (Pizza p in o.pizzaOrderList)
            {
                PizzaBoxData.data.Item newItem = new PizzaBoxData.data.Item();
                newItem.SetItem(p.size, p.crust, p.toppings, c.getLastOrderId(), p.numberOfPizza);
                c.AddItem(newItem);
            }
            Console.WriteLine($"Order ID#{c.getLastOrderId()} was placed successfully. Press Enter to continue...");
            mode = 0;
            string str= Console.ReadLine();
        }
        public void ViewOrderHistory(int uid)
        { 
            Console.WriteLine("-----------------------------Order History----------------------------------");
            Crud c = new Crud();
            foreach (PizzaBoxData.data.PizzaOrder po in c.getUserOrderHistory(uid))
            {
                po.DisplayOrderHistoryForCustomer();
            }
            Console.WriteLine("----------------------------------------------------------------------------");
        }
        public void UserViewOrderDetails(int uid, int oid)
        {
            Crud c = new Crud();
            if (c.OrderExist(oid))
            {
                if (uid==c.GetUserIDByOrderID(oid))
                {
                    int j = 1;
                    c.GetOrderByOderID(oid).DisplayOrderHistoryForCustomer();
                    foreach (PizzaBoxData.data.Item i in c.GetItemByOrderID(oid))
                    {
                        Console.WriteLine($"------------Item#{j}----------");
                        i.DisplayItemDetails();
                        Console.WriteLine($"------------------------------");
                        j++;
                    }
                }
                else
                { Console.WriteLine("You can't view an order that doesn't belong to you."); }
            }
            else
            {
                Console.WriteLine("Order doesn't exist.");
            }
        }
        public void AdminViewOrderDetails(int oid)
        {
            Crud c = new Crud();
            if (c.OrderExist(oid))
            {
                int j = 1;
                c.GetOrderByOderID(oid).DisplayOrderHistoryForCustomer();
                foreach (PizzaBoxData.data.Item i in c.GetItemByOrderID(oid))
                {
                    Console.WriteLine($"------------Item#{j}----------");
                    i.DisplayItemDetails();
                    Console.WriteLine($"------------------------------");
                    j++;
                }            
            }
            else
            {
                Console.WriteLine("Order doesn't exist.");
            }
        }
        public int inputValidation(int min, int max)
        {
            int s = 0;
            do
            {
                if (!(Int32.TryParse(Console.ReadLine(), out s) && s <= max && s >= min))
                { Console.WriteLine("Please enter a valid input."); }
            }
            while (s > max || s < min); // check range
            return s;
        }
        public string inputStringValidation(int minlen, int maxlen, int valueCheckType)
        {
            string str = "";
            bool CorrectValueType=true;
            do
            {
                str = Console.ReadLine();
                if (str == BackToMainMenu)
                { decision = BackToMainMenu;  }
                else
                {
                    if (str.Length > maxlen)
                    {
                        Console.WriteLine($"Maximun characters you can enter is {maxlen}, Please try again.");
                    }
                    else if (str.Length < minlen)
                    {
                        Console.WriteLine($"Minimun characters you can enter is {minlen}, Please try again.");
                    }
                    else
                    {
                        switch (valueCheckType)
                        {
                            case 1:
                                CorrectValueType = OnlyCharNum(str);
                                break;
                            case 2:
                                CorrectValueType = OnlyCharSpace(str);
                                break;
                            case 3:
                                CorrectValueType = OnlyNum(str);
                                break;
                            default:
                                break;
                        }

                    }
                }
            }
            while ((str.Length > maxlen|| str.Length < minlen|| !CorrectValueType)&& (decision!=BackToMainMenu));
            return str;
        }
        public bool OnlyCharNum(string str)
        {
            bool CorrectValueType;
            CorrectValueType = str.All(c => Char.IsLetterOrDigit(c) || c.Equals('_'));
            if (!CorrectValueType)
            {
                Console.WriteLine("Only Letters, numbers and underscore are allowed for this input. Please try again.");
            }
            return CorrectValueType;
        }
        public bool OnlyCharSpace(string str)
        {
            bool CorrectValueType;
            CorrectValueType = str.All(c => Char.IsLetter(c) || c.Equals(' '));
            if (!CorrectValueType)
            {
                Console.WriteLine("Only Letters and space are allowed for this input. Please try again.");
            }
            return CorrectValueType;
        }
        public bool OnlyNum(string str)
        {
            bool CorrectValueType;
            CorrectValueType = str.All(c => Char.IsDigit(c));
            if (!CorrectValueType)
            {
                Console.WriteLine("Only digits are allowed for this input. Please try again.");
            }
            return CorrectValueType;
        }

    }
}
