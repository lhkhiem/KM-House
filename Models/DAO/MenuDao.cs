using Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class MenuDao
    {
        DBContext db = null;
        public MenuDao()
        {
            db = new DBContext();
        }
        public IEnumerable<Menu> ListAll()
        {
            return db.Menus.ToList();
        }
        public int Insert(Menu entity)
        {
            entity.DisplayOrder = GetMaxDislayOrder(entity.MenuTypeID)+1;
            db.Menus.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
        public int Update(Menu entity)
        {
            var model = db.Menus.Find(entity.ID);
            model.Text = entity.Text;
            model.Link = entity.Link;
            model.Target = entity.Target;
            db.SaveChanges();
            return entity.ID;
        }
        public Menu GetByID(int id)
        {
            return db.Menus.Find(id);
        }
        public bool CheckIDExist(int id)
        {
            var model = db.Menus.Find(id);
            if (model == null)//không tồn tại
                return true;
            else return false;
        }
        public bool Delete(int id)
        {
            try
            {
                var menutype = db.Menus.Find(id);
                db.Menus.Remove(menutype);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool ChangeStatus(int id)
        {
            var menu = db.Menus.Find(id);
            menu.Status = !menu.Status;
            db.SaveChanges();
            return menu.Status;
        }
        public bool ChangeOrder(int id, int order, int menuType)
        {
            var menu = db.Menus.Find(id);
            var item = db.Menus.Where(x => x.DisplayOrder == order && x.MenuTypeID == menuType).ToList();
            if (item.Count == 0)
            {
                menu.DisplayOrder = order;
                db.SaveChanges();
                return true;
            }
            else
                return false;
        }
        public int GetMaxDislayOrder(int menuType)
        {
            var model = db.Menus.Where(x => x.MenuTypeID == menuType).OrderByDescending(x => x.DisplayOrder).FirstOrDefault();
            if (model != null)
            {
                return model.DisplayOrder; ;
            } 
            else return 0;
        }
    }
}
