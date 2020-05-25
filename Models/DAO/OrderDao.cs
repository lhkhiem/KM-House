using Models.EF;
using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class OrderDao
    {
        DBContext db = null;
        public OrderDao()
        {
            db = new DBContext();
        }
        public IEnumerable<OrderViewModel> ListAll()
        {
            var model = from a in db.Orders
                        join b in db.Users
                        on a.UserID equals b.ID
                        select new OrderViewModel()
                        {
                            ID = a.ID,
                            UserID = b.ID,
                            NameAccount = b.Name,
                            CreateDate = a.CreateDate,
                            ShipName = a.ShipName,
                            ShipAddress = a.ShipAddress,
                            ShipMobile = a.ShipMobile,
                            ShipEmail = a.ShipEmail,
                            Status = a.Status
                        };
            return model.ToList();
        }
        public long Insert(Order order)
        {
            db.Orders.Add(order);
            db.SaveChanges();
            return order.ID;
        }
        public int ChangeStatus(long id)
        {
            var order = db.Orders.Find(id);
            var status= order.Status;
            if(status == 2) {status = 0; } else { status++; }
            order.Status = status;
            db.SaveChanges();
            return order.Status;
        }
        public bool Delete(long id)
        {
            try
            {
                var order = db.Orders.Find(id);
                var detail = db.OrderDetails.Where(x => x.OrderID == (id)).ToList();
                foreach (var item in detail)
                {
                    db.OrderDetails.Remove(item);
                }
                db.Orders.Remove(order);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
