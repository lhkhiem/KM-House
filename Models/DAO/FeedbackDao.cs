using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class FeedbackDao
    {
        DBContext db = null;
        public FeedbackDao()
        {
            db = new DBContext();
        }
        public IEnumerable<Feedback> ListAll()
        {
            return db.Feedbacks.ToList();
        }
        public Feedback GetById(int id)
        {
            return db.Feedbacks.Find(id);
        }
        public long Insert(Feedback entity)
        {
            db.Feedbacks.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
        public bool Delete(int id)
        {
            try
            {
                var entity = db.Feedbacks.Find(id);
                    db.Feedbacks.Remove(entity);
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
            var res = db.Feedbacks.Find(id);
            res.Status = !res.Status;
            db.SaveChanges();
            return res.Status;
        }
    }
}
