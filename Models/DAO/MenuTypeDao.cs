using Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class MenuTypeDao
    {
        DBContext db = null;
        public MenuTypeDao()
        {
            db = new DBContext();
        }
        public IEnumerable<MenuType> ListAll()
        {
            return db.MenuTypes.ToList();
        }
        public int Insert(MenuType entity)
        {
            db.MenuTypes.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
        public int Update(MenuType entity)
        {
            var model = db.MenuTypes.Find(entity.ID);
            model.Name = entity.Name;
            db.SaveChanges();
            return entity.ID;
        }
        public MenuType GetByID(int id)
        {
            return db.MenuTypes.Find(id);
        }
        public bool CheckIDExist(int id)
        {
            var model = db.MenuTypes.Find(id);
            if (model == null)//không tồn tại
                return true;
            else return false;
        }
        public bool Delete(int id)
        {
            try
            {
                var menutype = db.MenuTypes.Find(id);
                db.MenuTypes.Remove(menutype);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool CheckMenuIsUsed(int id)
        {
            var model = db.Menus.FirstOrDefault(x => x.MenuTypeID == id);
            if (model == null)
                return false;//khong tim thay
            else return true;//tim thay
        }
    }
}
