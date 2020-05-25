using Common;
using Models.EF;
using Models.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class ProductDao
    {
        DBContext db = null;
        public ProductDao()
        {
            db = new DBContext();
        }
        public IEnumerable<ProductViewModel> ListAll()
        {
            var model = from a in db.Products
                        join b in db.ProductCategories
                        on a.ProductCategoryID equals b.ID
                        join c in db.Units
                        on a.UnitID equals c.ID
                        select new ProductViewModel()
                        {
                            ID = a.ID,
                            Name = a.Name,
                            Code = a.Code,
                            Description = a.Description,
                            Image = a.Image,
                            MetaDescriptions = a.MetaDescriptions,
                            MetaKeywords = a.MetaKeywords,
                            MetaTitle = a.MetaTitle,
                            CreateDate = a.CreateDate,
                            CreateBy = a.CreateBy,
                            ModifiedBy = a.ModifiedBy,
                            ModifiedDate = a.ModifiedDate,
                            MoreImage = a.MoreImage,
                            Price = a.Price,
                            ProductCategoryName = b.Name,
                            PromotionPrice = a.PromotionPrice,
                            Quantity = a.Quantity,
                            UnitName = c.Name,
                            Warranty = a.Warranty,
                            Status = a.Status,
                            TopHot = a.TopHot,
                            MetaTitleProductCategory = b.MetaTitle,
                            ProductCategoryID = b.ID,
                            Detail = a.Detail,
                            Tag = a.Tag
                        };
            return model.ToList();
        }
        public List<ProductViewModel> ListAllByTag(string tagId)
        {
            var model = from a in db.Products
                        join b in db.ProductCategories
                        on a.ProductCategoryID equals b.ID
                        join c in db.Units
                        on a.UnitID equals c.ID
                        join d in db.ProductTags on a.ID equals d.ProductID
                        where d.TagID == tagId
                        select new ProductViewModel()
                        {
                            ID = a.ID,
                            Name = a.Name,
                            Code = a.Code,
                            Description = a.Description,
                            Image = a.Image,
                            MetaDescriptions = a.MetaDescriptions,
                            MetaKeywords = a.MetaKeywords,
                            MetaTitle = a.MetaTitle,
                            CreateDate = a.CreateDate,
                            CreateBy = a.CreateBy,
                            ModifiedBy = a.ModifiedBy,
                            ModifiedDate = a.ModifiedDate,
                            MoreImage = a.MoreImage,
                            Price = a.Price,
                            ProductCategoryName = b.Name,
                            PromotionPrice = a.PromotionPrice,
                            Quantity = a.Quantity,
                            UnitName = c.Name,
                            Warranty = a.Warranty,
                            Status = a.Status,
                            TopHot = a.TopHot,
                            MetaTitleProductCategory = b.MetaTitle,
                            ProductCategoryID = b.ID,
                            Detail = a.Detail,
                            Tag = a.Tag
                        };
            return model.ToList();
        }
        public bool Insert(Product entity)
        {
            try
            {
                entity.Status = true;
                entity.CreateDate = DateTime.Now;
                db.Products.Add(entity);
                db.SaveChanges();
                if (!string.IsNullOrEmpty(entity.Tag))
                {
                    string[] tags = entity.Tag.Split(',');
                    foreach (var tag in tags)
                    {
                        var tagId = StringHelper.ToUnsignString(tag);
                        var existedTag = this.CheckTag(tagId);
                        var existedProduct = this.CheckProductTag(entity.ID, tagId);

                        if (!existedTag)
                        {
                            //insert to Tag table
                            this.InsertTag(tagId, tag);

                        }
                        //Insert to ProductTag table
                        if (!existedProduct)
                        {
                            this.InsertProductTag(entity.ID, tagId);
                        }

                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Update(Product entity)
        {
            try
            {
                var model = db.Products.Find(entity.ID);
                model.Name = entity.Name;
                model.Code = entity.Code;
                model.Detail = entity.Detail;
                model.Description = entity.Description;
                model.Image = entity.Image;
                model.MetaDescriptions = entity.MetaDescriptions;
                model.MetaKeywords = entity.MetaKeywords;
                model.MetaTitle = entity.MetaTitle;
                model.ModifiedBy = entity.ModifiedBy;
                model.ModifiedDate = DateTime.Now;
                model.MoreImage = entity.MoreImage;
                model.Price = entity.Price;
                model.ProductCategoryID = entity.ProductCategoryID;
                model.PromotionPrice = entity.PromotionPrice;
                model.Quantity = entity.Quantity;
                model.UnitID = entity.UnitID;
                model.Warranty = entity.Warranty;
                model.TopHot = entity.TopHot;
                model.Tag = entity.Tag;
                db.SaveChanges();
                if (!string.IsNullOrEmpty(entity.Tag))
                {
                    string[] tags = entity.Tag.Split(',');
                    foreach (var tag in tags)
                    {
                        var tagId = StringHelper.ToUnsignString(tag);
                        var existedTag = this.CheckTag(tagId);
                        var existedProduct = this.CheckProductTag(entity.ID, tagId);

                        if (!existedTag)
                        {
                            //insert to Tag table
                            this.InsertTag(tagId, tag);

                        }
                        //Insert to ProductTag table
                        if (!existedProduct)
                        {
                            this.InsertProductTag(entity.ID, tagId);
                        }

                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public Product GetByID(long id)
        {
            return db.Products.Find(id);
        }
        public IEnumerable<ProductViewModel> GetListByIDView(long id)
        {
            var model = from a in db.Products
                        join b in db.ProductCategories
                        on a.ProductCategoryID equals b.ID
                        join c in db.Units
                        on a.UnitID equals c.ID
                        select new ProductViewModel()
                        {
                            ID = a.ID,
                            Name = a.Name,
                            Code = a.Code,
                            Description = a.Description,
                            Image = a.Image,
                            MetaDescriptions = a.MetaDescriptions,
                            MetaKeywords = a.MetaKeywords,
                            MetaTitle = a.MetaTitle,
                            CreateDate = a.CreateDate,
                            CreateBy = a.CreateBy,
                            ModifiedBy = a.ModifiedBy,
                            ModifiedDate = a.ModifiedDate,
                            MoreImage = a.MoreImage,
                            Price = a.Price,
                            ProductCategoryName = b.Name,
                            PromotionPrice = a.PromotionPrice,
                            Quantity = a.Quantity,
                            UnitName = c.Name,
                            Warranty = a.Warranty,
                            Status = a.Status,
                            TopHot = a.TopHot,
                            MetaTitleProductCategory = b.MetaTitle,
                            ProductCategoryID = b.ID,
                            Detail = a.Detail,
                            UnitID = c.ID,
                            Tag = a.Tag
                        };
            return model.Where(x => x.ID.Equals(id)).ToList();
        }
        public ProductViewModel GetByIDView(long id)
        {
            var model = from a in db.Products.Where(x => x.ID.Equals(id))
                        join b in db.ProductCategories
                        on a.ProductCategoryID equals b.ID
                        join c in db.Units
                        on a.UnitID equals c.ID
                        select new ProductViewModel()
                        {
                            ID = a.ID,
                            Name = a.Name,
                            Code = a.Code,
                            Description = a.Description,
                            Image = a.Image,
                            MetaDescriptions = a.MetaDescriptions,
                            MetaKeywords = a.MetaKeywords,
                            MetaTitle = a.MetaTitle,
                            CreateDate = a.CreateDate,
                            CreateBy = a.CreateBy,
                            ModifiedBy = a.ModifiedBy,
                            ModifiedDate = a.ModifiedDate,
                            MoreImage = a.MoreImage,
                            Price = a.Price,
                            ProductCategoryName = b.Name,
                            PromotionPrice = a.PromotionPrice,
                            Quantity = a.Quantity,
                            UnitName = c.Name,
                            Warranty = a.Warranty,
                            Status = a.Status,
                            TopHot = a.TopHot,
                            MetaTitleProductCategory = b.MetaTitle,
                            ProductCategoryID = b.ID,
                            Detail = a.Detail,
                            UnitID = c.ID,
                            Tag = a.Tag
                        };
            return model.FirstOrDefault();
        }
        public bool Delete(long id)
        {
            try
            {
                var product = db.Products.Find(id);
                db.Products.Remove(product);
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
            var product = db.Products.Find(id);
            product.Status = !product.Status;
            db.SaveChanges();
            return product.Status;
        }
        public void UpdateImages(long productId, string images)
        {
            var product = db.Products.Find(productId); ;

            product.MoreImage = images;
            db.SaveChanges();

        }
        public List<ProductViewModel> ListRelateProduct(long productId)
        {
            var product = this.GetByIDView(productId);
            var listProduct = this.ListAll();
            return listProduct.Where(x => x.ID != productId && x.ProductCategoryID == product.ProductCategoryID).ToList();
        }
        public List<string> ListName(string keyword)
        {
            return db.Products.Where(x => x.Name.Contains(keyword)).Select(x => x.Name).ToList();
        }
        public void InsertTag(string id, string name)
        {
            var tag = new Tag();
            tag.ID = id;
            tag.Name = name;
            db.Tags.Add(tag);
            db.SaveChanges();
        }
        public void InsertProductTag(long productId, string tagId)
        {
            var productTag = new ProductTag();
            productTag.ProductID = productId;
            productTag.TagID = tagId;
            db.ProductTags.Add(productTag);
            db.SaveChanges();
        }
        public bool CheckTag(string id)
        {
            return db.Tags.Count(x => x.ID == id) > 0;
        }
        public bool CheckProductTag(long productId, string tagId)
        {
            return db.ProductTags.Count(x => x.TagID == tagId && x.ProductID == productId) > 0;
        }
        public void RemoveAllProductTag(long productId)
        {
            db.ProductTags.RemoveRange(db.ProductTags.Where(x => x.ProductID == productId));
            db.SaveChanges();
        }
        public List<Tag> ListTag(long productId)
        {
            var model = (from a in db.Tags
                         join b in db.ProductTags on a.ID equals b.TagID
                         where b.ProductID == productId
                         select new
                         {
                             ID = b.TagID,
                             Name = a.Name
                         }).AsEnumerable().Select(x => new Tag()
                         {
                             ID = x.ID,
                             Name = x.Name
                         });
            return model.ToList();
        }
        public Tag GetTag(string id)
        {
            return db.Tags.Find(id);
        }
    }
}
