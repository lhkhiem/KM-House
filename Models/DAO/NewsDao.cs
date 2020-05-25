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
    public class NewsDao
    {
        DBContext db = null;
        public NewsDao()
        {
            db = new DBContext();
        }
        public IEnumerable<NewsViewModel> ListAll()
        {
            var model = from a in db.News
                        join b in db.NewsCategories
                        on a.NewsCategoryID equals b.ID
                        select new NewsViewModel()
                        {
                            ID = a.ID,
                            Name = a.Name,
                            Description = a.Description,
                            Image = a.Image,
                            MetaDescriptions = a.MetaDescriptions,
                            MetaKeywords = a.MetaKeywords,
                            MetaTitle = a.MetaTitle,
                            CreateDate = a.CreateDate,
                            CreateBy = a.CreateBy,
                            ModifiedBy = a.ModifiedBy,
                            ModifiedDate = a.ModifiedDate,
                            Status = a.Status,
                            Detail = a.Detail,
                            Tag = a.Tag,
                            NewsCategoryID = a.NewsCategoryID,
                            NewsCategoryName = b.Name,
                            NewsCategoryMetaTitle = b.MetaTitle
                        };
            return model.ToList();
        }
        public List<NewsViewModel> ListAllByTag(string tagId)
        {
            var model = (from a in db.News
                         join b in db.NewsTags on a.ID equals b.NewsID
                         join c in db.NewsCategories on a.NewsCategoryID equals c.ID
                         where b.TagID == tagId
                         select new
                         {
                             Name = a.Name,
                             MetaTitle = a.MetaTitle,
                             Image = a.Image,
                             Description = a.Description,
                             CreateDate = a.CreateDate,
                             CreateBy = a.CreateBy,
                             ID = a.ID,
                             NewsCategoryName=c.Name
                         }).AsEnumerable().Select(x => new NewsViewModel()
                         {
                             Name = x.Name,
                             MetaTitle = x.MetaTitle,
                             Image = x.Image,
                             Description = x.Description,
                             CreateBy=x.CreateBy,
                             CreateDate=x.CreateDate,
                             ID=x.ID,
                             NewsCategoryName=x.NewsCategoryName
                         });
            return model.ToList();
        }
        public List<NewsViewModel> ListAllByCategory(long cateId)
        {
            var model = (from a in db.News
                         join c in db.NewsCategories on a.NewsCategoryID equals c.ID
                         where c.ID == cateId
                         select new
                         {
                             Name = a.Name,
                             MetaTitle = a.MetaTitle,
                             Image = a.Image,
                             Description = a.Description,
                             CreateDate = a.CreateDate,
                             CreateBy = a.CreateBy,
                             ID = a.ID,
                             NewsCategoryName = c.Name
                         }).AsEnumerable().Select(x => new NewsViewModel()
                         {
                             Name = x.Name,
                             MetaTitle = x.MetaTitle,
                             Image = x.Image,
                             Description = x.Description,
                             CreateBy = x.CreateBy,
                             CreateDate = x.CreateDate,
                             ID = x.ID,
                             NewsCategoryName = x.NewsCategoryName
                         });
            return model.ToList();
        }
        public bool Insert(News entity)
        {
            try
            {
                entity.Status = true;
                entity.CreateDate = DateTime.Now;
                entity.MetaKeywords = entity.MetaTitle;
                entity.MetaDescriptions = entity.MetaTitle;
                db.News.Add(entity);
                db.SaveChanges();
                if (!string.IsNullOrEmpty(entity.Tag))
                {
                    string[] tags = entity.Tag.Split(',');
                    foreach (var tag in tags)
                    {
                        var tagId = StringHelper.ToUnsignString(tag);
                        var existedTag = this.CheckTag(tagId);
                        var existedNews = this.CheckNewsTag(entity.ID, tagId);

                        if (!existedTag)
                        {
                            //insert to Tag table
                            this.InsertTag(tagId, tag);

                        }
                        //Insert to NewsTag table
                        if (!existedNews)
                        {
                            this.InsertNewsTag(entity.ID, tagId);
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
        public bool Update(News entity)
        {
            try
            {
                var model = db.News.Find(entity.ID);
                model.Name = entity.Name;
                model.Description = entity.Description;
                model.Image = entity.Image;
                model.MetaDescriptions = entity.MetaDescriptions;
                model.MetaKeywords = entity.MetaKeywords;
                model.MetaTitle = entity.MetaTitle;
                model.ModifiedBy = entity.ModifiedBy;
                model.ModifiedDate = DateTime.Now;
                model.Detail = entity.Detail;
                model.Tag = entity.Tag;
                model.NewsCategoryID = entity.NewsCategoryID;
                db.SaveChanges();
                if (!string.IsNullOrEmpty(entity.Tag))
                {
                    this.RemoveAllNewsTag(entity.ID);
                    string[] tags = entity.Tag.Split(',');
                    foreach (var tag in tags)
                    {
                        var tagId = StringHelper.ToUnsignString(tag);
                        var existedTag = this.CheckTag(tagId);
                        var existedNews = this.CheckNewsTag(entity.ID, tagId);

                        if (!existedTag)
                        {
                            //insert to Tag table
                            this.InsertTag(tagId, tag);

                        }
                        //Insert to NewsTag table
                        if (!existedNews)
                        {
                            this.InsertNewsTag(entity.ID, tagId);
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
        public News GetByID(long id)
        {
            return db.News.Find(id);
        }
        public NewsViewModel GetByIDView(long id)
        {
            var model = from a in db.News.Where(x => x.ID.Equals(id))
                        join b in db.NewsCategories
                        on a.NewsCategoryID equals b.ID
                        select new NewsViewModel()
                        {
                            ID = a.ID,
                            Name = a.Name,
                            Description = a.Description,
                            Image = a.Image,
                            MetaDescriptions = a.MetaDescriptions,
                            MetaKeywords = a.MetaKeywords,
                            MetaTitle = a.MetaTitle,
                            CreateDate = a.CreateDate,
                            CreateBy = a.CreateBy,
                            ModifiedBy = a.ModifiedBy,
                            ModifiedDate = a.ModifiedDate,
                            Status = a.Status,
                            Detail = a.Detail,
                            Tag = a.Tag,
                            NewsCategoryID = a.NewsCategoryID,
                            NewsCategoryName = b.Name,
                            NewsCategoryMetaTitle = b.MetaTitle
                        };
            return model.FirstOrDefault();
        }
        public bool Delete(long id)
        {
            try
            {
                var entity = db.News.Find(id);
                db.News.Remove(entity);
                db.SaveChanges();
                this.RemoveAllNewsTag(id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool ChangeStatus(long id)
        {
            var entity = db.News.Find(id);
            entity.Status = !entity.Status;
            db.SaveChanges();
            return entity.Status;
        }
        public void InsertTag(string id, string name)
        {
            var tag = new Tag();
            tag.ID = id;
            tag.Name = name;
            db.Tags.Add(tag);
            db.SaveChanges();
        }
        public void InsertNewsTag(long newsId, string tagId)
        {
            var newsTag = new NewsTag();
            newsTag.NewsID = newsId;
            newsTag.TagID = tagId;
            db.NewsTags.Add(newsTag);
            db.SaveChanges();
        }
        public bool CheckTag(string id)
        {
            return db.Tags.Count(x => x.ID == id) > 0;
        }
        public bool CheckNewsTag(long newsId, string tagId)
        {
            return db.NewsTags.Count(x => x.TagID == tagId && x.NewsID == newsId) > 0;
        }
        public void RemoveAllNewsTag(long newsId)
        {
            db.NewsTags.RemoveRange(db.NewsTags.Where(x => x.NewsID == newsId));
            db.SaveChanges();
        }
        public List<Tag> ListTag(long newsId)
        {
            var model = (from a in db.Tags
                         join b in db.NewsTags on a.ID equals b.TagID
                         where b.NewsID == newsId
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
        public List<NewsViewModel> ListRelateNews(long id)
        {
            var entity = this.GetByIDView(id);
            var list = this.ListAll();
            return list.Where(x => x.ID != id && x.NewsCategoryID.Equals(entity.NewsCategoryID)).ToList();
        }
        public List<NewsViewModel> ListRecentNews(int number)
        {
            return this.ListAll().OrderByDescending(x => x.CreateDate).Take(number).ToList();
        }
        public List<string> ListName(string keyword)
        {
            return db.News.Where(x => x.Name.Contains(keyword)).Select(x => x.Name).ToList();
        }
    }
}
