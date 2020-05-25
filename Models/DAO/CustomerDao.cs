using Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class CustomerDao
    {
        DBContext db = null;
        public CustomerDao()
        {
            db = new DBContext();
        }
        public IEnumerable<Customer> ListAll()
        {
            return db.Customers.ToList();
        }
        //Danh sách hiển thị phân trang
        public IEnumerable<Customer> ListAllPaging(int sltSearch, string searchString, int page, int pageSize)
        {
            IQueryable<Customer> model = db.Customers;
            if (sltSearch == 1)
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    model = model.Where(x => x.Name.Contains(searchString) || x.Ogran.Contains(searchString) || x.Email.Contains(searchString) || 
                    x.Phone.Contains(searchString) || x.Address.Contains(searchString));
                }
            }
            if (sltSearch == 2)
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    model = model.Where(x => x.Name.Contains(searchString));
                }
            }
            if (sltSearch == 3)
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    model = model.Where(x => x.Ogran.Contains(searchString));
                }
            }
            if (sltSearch == 4)
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    model = model.Where(x => x.Address.Contains(searchString));
                }
            }
            if (sltSearch == 5)
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    model = model.Where(x => x.Email.Contains(searchString));
                }
            }
            if (sltSearch == 6)
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    model = model.Where(x => x.Phone.Contains(searchString));
                }
            }
            if (pageSize == 0) pageSize = model.Count();
            return model.OrderByDescending(x => x.Status).ThenByDescending(x => x.ID).ToPagedList(page, pageSize);
        }
        public long Insert(Customer entity)
        {
            db.Customers.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
        public bool Update(Customer entity)
        {
            try
            {
                var cus = db.Customers.Find(entity.ID);
                cus.Name = entity.Name;
                cus.Ogran = entity.Ogran;
                cus.Address = entity.Address;
                cus.Email = entity.Email;
                cus.Phone = entity.Phone;
                cus.Status = entity.Status;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        //Truyền dữ liệu vào form Edit
        public Customer GetInfoById(int id)
        {
            return db.Customers.Find(id);
        }
        public bool Delete(int id)
        {
            try
            {
                var cus = db.Customers.Find(id);
                db.Customers.Remove(cus);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public bool ChangeStatus(long id)
        {
            var cus = db.Customers.Find(id);
            cus.Status = !cus.Status;
            db.SaveChanges();
            return cus.Status;
        }
    }
}
