using Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class FooterDao
    {
        DBContext db = null;
        public FooterDao()
        {
            db = new DBContext();
        }
        public IEnumerable<Footer> ListAll()
        {
            return db.Footers.ToList();
        }
        public bool Insert(Footer entity)
        {
            try
            {
                entity.Status = false;
                db.Footers.Add(entity);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Update(Footer entity)
        {
            try
            {
                var model = db.Footers.Find(entity.ID);
                model.Name = entity.Name;
                model.Content = entity.Content;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }
        public Footer GetByID(int id)
        {
            return db.Footers.Find(id);
        }
        public bool Delete(int id)
        {
            try
            {
                var footer = db.Footers.Find(id);
                if (footer.Status != true)
                {
                    db.Footers.Remove(footer);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool ChangeStatus(int id)
        {
            var footer = db.Footers.Find(id);
            var listFooter = db.Footers.ToList();
            footer.Status = true;
            foreach(var item in listFooter)
            {
                if ((item.ID)!=(id))
                    item.Status = false;
            }
            db.SaveChanges();
            return footer.Status;
        }

    }
}
