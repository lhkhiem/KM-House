using Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class ProductCategoryDao
    {
        DBContext db = null;
        public ProductCategoryDao()
        {
            db = new DBContext();
        }
        public IEnumerable<ProductCategory> ListAll()
        {            
            return  db.ProductCategories.ToList();
        }
        public long Insert(ProductCategory entity)
        {
            if (entity.ParentID == null) entity.DisplayOrder = GetMaxDislayOrder() + 1;
            else entity.DisplayOrder = 0;

            entity.CreateDate = DateTime.Now;
            db.ProductCategories.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
        public long Update(ProductCategory entity)
        {
            var model = db.ProductCategories.Find(entity.ID);
            model.Name = entity.Name;
            model.ParentID = entity.ParentID;
            model.ModifiedBy = entity.CreateBy;
            model.ModifiedDate = DateTime.Now;
            model.MetaTitle = entity.MetaTitle;
            db.SaveChanges();
            return entity.ID;
        }
        public ProductCategory GetByID(long id)
        {
            return db.ProductCategories.Find(id);
        }
        public bool CheckIDExist(long id)
        {
            var model = db.ProductCategories.Find(id);
            if (model == null)//không tồn tại
                return true;
            else return false;
        }
        public bool HasChild(long ?id)
        {
            var model = db.ProductCategories.Find(id);

            if (model.ParentID != null)//không có menu con
                return true;
            else return false;
        }
        public bool GetChild(long id)
        {
            var model = db.ProductCategories.Where(x=>x.ParentID==id);

            if (model.Count()>0)
                return true;//có có menu con
            else return false;//không có menu con
        }
        public bool Delete(long id)
        {
            try
            {
                var productCategory = db.ProductCategories.Find(id);
                var listChild = db.ProductCategories.Where(x => x.ParentID == productCategory.ID);
                db.ProductCategories.RemoveRange(listChild);
                db.ProductCategories.Remove(productCategory);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool CheckProductIsUsed(long id)
        {
            var model = db.Products.FirstOrDefault(x => x.ProductCategoryID == id);
            if (model == null)
                return false;//khong tim thay
            else return true;//tim thay
        }
        public int GetMaxDislayOrder()
        {
            var model = db.ProductCategories.Where(x=>x.ParentID==null).OrderByDescending(x => x.DisplayOrder).FirstOrDefault();
            if (model != null)
            {
                return model.DisplayOrder; ;
            }
            else return 0;
        }
        public bool ChangeOrder(int id, int order)
        {
            var menu = db.ProductCategories.Find(id);
            var item = db.ProductCategories.Where(x => x.DisplayOrder == order).ToList();
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
