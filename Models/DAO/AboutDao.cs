using Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class AboutDao
    {
        DBContext db = null;
        public AboutDao()
        {
            db = new DBContext();
        }
        public About GetAbout()
        {
            return db.Abouts.FirstOrDefault();
        }
        public int Update(About entity)
        {
            var about = db.Abouts.FirstOrDefault();
            var count = db.Abouts.Count();
            if (count == 0)
            {
                about.Name = entity.Name;
                about.Image = entity.Image;
                about.MetaDescriptions = entity.MetaDescriptions;
                about.MetaKeywords = entity.MetaKeywords;
                about.MetaTitle = entity.MetaTitle;
                about.CreateDate = DateTime.Now;
                about.CreateBy = entity.CreateBy;
                about.Description = entity.Description;
                about.Detail = entity.Detail;
                about.Status = entity.Status;
                db.Abouts.Add(about);
            }
            else
            {
                about.Name = entity.Name;
                about.Image = entity.Image;
                about.MetaDescriptions = entity.MetaDescriptions;
                about.MetaKeywords = entity.MetaKeywords;
                about.MetaTitle = entity.MetaTitle;
                about.ModifiedBy = entity.ModifiedBy;
                about.ModifiedDate = DateTime.Now;
                about.Description = entity.Description;
                about.Detail = entity.Detail;
                about.Status = entity.Status;
            }
            
            db.SaveChanges();
            return entity.ID;
        }
        
    }
}
