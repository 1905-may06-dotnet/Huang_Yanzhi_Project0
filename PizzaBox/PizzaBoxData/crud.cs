using System;
using System.Collections.Generic;
using System.Text;
using PizzaBoxData.data;
using System.Linq;

namespace PizzaBoxData
{
    public class Crud
    {
        public AppUser GetUsers(int id)
        {
            var user = DbInstance.Instance.AppUser.Where<AppUser>(r => r.UserId == id).FirstOrDefault();
            return user;
        }
        public bool UsernameExist(string un)
        {
            bool Exist = DbInstance.Instance.AppUser.Any(r => r.UserName == un);
            return Exist;
        }
        public bool OrderExist(int oid)
        {
            bool Exist = DbInstance.Instance.PizzaOrder.Any(o => o.OrderId == oid);
            return Exist;
        }
        public int GetUserIDByOrderID(int oid)
        {
           return (int)(DbInstance.Instance.PizzaOrder.Where<PizzaOrder>(r => r.OrderId == oid).FirstOrDefault().UserId);
        }
        public PizzaOrder GetOrderByOderID(int oid)
        {
            return DbInstance.Instance.PizzaOrder.Where<PizzaOrder>(r => r.OrderId == oid).FirstOrDefault();
        }
        public bool PasswordMatched(string un, string pw)
        {
            AppUser user = new AppUser();
            user = DbInstance.Instance.AppUser.Where<AppUser>(r => r.UserName == un).FirstOrDefault();
            return pw == user.UserPassword;
        }
        public void AddUser(AppUser r)
        {
            DbInstance.Instance.AppUser.Add(r);
            DbInstance.Instance.SaveChanges();
            Console.WriteLine($"User {r.UserName} has registered successfully.");
        }
        public void DisplayAllLocations()
        {
            List<Location> locationList = DbInstance.Instance.Location.ToList();
            foreach (Location l in locationList)
            {
                l.displayDetails();
            }
        }
        public Location GetLocation(int id)
        {
            return DbInstance.Instance.Location.Where<Location>(r => r.LocationId == id).FirstOrDefault();
        }
        public void AddOrder(PizzaOrder po)
        {
            DbInstance.Instance.PizzaOrder.Add(po);
            DbInstance.Instance.SaveChanges();           
        }
        public void AddItem(Item i)
        {
            DbInstance.Instance.Item.Add(i);
            DbInstance.Instance.SaveChanges();
        }
        public int getLastOrderId()
        {
            var lastOrder = DbInstance.Instance.PizzaOrder.FirstOrDefault(p => p.TimeDate == DbInstance.Instance.PizzaOrder.Max(x => x.TimeDate));
            return lastOrder.OrderId;
        }
        public int getUserId(string un)
        {
           return DbInstance.Instance.AppUser.Where<AppUser>(r => r.UserName == un).FirstOrDefault().UserId;
        }
        public string getUserFullName(string un)
        {
            return DbInstance.Instance.AppUser.Where<AppUser>(r => r.UserName == un).FirstOrDefault().FullName;
        }
        public List<PizzaOrder> getUserOrderHistory(int uid)
        {
            return DbInstance.Instance.PizzaOrder.Where<PizzaOrder>(r => r.UserId == uid).ToList();
        }
        public bool UserHasOrder(int uid)
        {
            return DbInstance.Instance.PizzaOrder.Any(r => r.UserId == uid);
        }
        public DateTime getUserOrderLastOrderTime(int uid)//new version getting last order's time
        {
            List<PizzaOrder> usersOrders = DbInstance.Instance.PizzaOrder.Where<PizzaOrder>(r => r.UserId == uid).ToList();
            List<string> dates = new List<string>();
            foreach (PizzaOrder u in usersOrders)
            {
                dates.Add(u.TimeDate);
            }
            DateTime dt = Convert.ToDateTime(dates.Max());
            return dt;
        }
        public int getUserOrderLastOrderLocationID(int uid)
        {
            return (int)(DbInstance.Instance.PizzaOrder.Where<PizzaOrder>(r => r.UserId == uid).FirstOrDefault(p => p.TimeDate == DbInstance.Instance.PizzaOrder.Max(x => x.TimeDate)).LocationId);
        }
        public AppUser getCustomerFromOrder(int uid)
        {
            return DbInstance.Instance.AppUser.Where<AppUser>(r => r.UserId== uid).FirstOrDefault();
        }
        public List<PizzaOrder> getLocationOrderHistory(int lid)
        {
            return DbInstance.Instance.PizzaOrder.Where<PizzaOrder>(r => r.LocationId == lid).ToList();
        }
        public List <Item> GetItemByOrderID(int oid)
        {
            return DbInstance.Instance.Item.Where<Item>(r => r.OrderId == oid).ToList();
        }

    }
}
