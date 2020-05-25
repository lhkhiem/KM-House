using Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class NewsCategoryDao
    {
        DBContext db = null;
        public NewsCategoryDao()
        {
            db = new DBContext();
        }
        public IEnumerable<NewsCategory> ListAll()
        {            
            return  db.NewsCategories.ToList();
        }
        public long Insert(NewsCategory entity)
        {
            if (entity.ParentID == null) entity.DisplayOrder = GetMaxDislayOrder() + 1;
            else entity.DisplayOrder = 0;

            entity.CreateDate = DateTime.Now;
            db.NewsCategories.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
        public long Update(NewsCategory entity)
        {
            var model = db.NewsCategories.Find(entity.ID);
            model.Name = entity.Name;
            model.ParentID = entity.ParentID;
            model.ModifiedBy = entity.CreateBy;
            model.ModifiedDate = DateTime.Now;
            model.MetaTitle = entity.MetaTitle;
            db.SaveChanges();
            return entity.ID;
        }
        public NewsCategory GetByID(long id)
        {
            return db.NewsCategories.Find(id);
        }
        public bool CheckIDExist(long id)
        {
            var model = db.NewsCategories.Find(id);
            if (model == null)//không tồn tại
                return true;
            else return false;
        }
        public bool IsSubMenu(long ?id)
        {
            var model = db.NewsCategories.Find(id);

            if (model.ParentID != null)//không có menu con
                return true;
            else return false;
        }
        public bool Delete(long id)
        {
            try
            {
                var newsCategory = db.NewsCategories.Find(id);
                db.NewsCategories.Remove(newsCategory);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool CheckNewsIsUsed(long id)
        {
            var model = db.News.FirstOrDefault(x => x.NewsCategoryID == id);
            if (model == null)
                return false;//khong tim thay
            else return true;//tim thay
        }
        public int GetMaxDislayOrder()
        {
            var model = db.NewsCategories.Where(x=>x.ParentID==null).OrderByDescending(x => x.DisplayOrder).FirstOrDefault();
            if (model != null)
            {
                return model.DisplayOrder; ;
            }
            else return 0;
        }
        public bool ChangeOrder(int id, int order)
        {
            var menu = db.NewsCategories.Find(id);
            var item = db.NewsCategories.Where(x => x.DisplayOrder == order).ToList();
            if (item.Count == 0)
            {
                menu.DisplayOrder = order;
                db.SaveChanges();
                return true;
            }
            else
                return false;
        }
    }
}
