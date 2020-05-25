using Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class UserGroupDao
    {
        DBContext db = null;
        public UserGroupDao()
        {
            db = new DBContext();
        }
        public IEnumerable<UserGroup> ListAll()
        {
            return db.UserGroups.ToList();
        }
        public string Insert(UserGroup entity)
        {
            db.UserGroups.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
        public string Update(UserGroup entity)
        {
            var model = db.UserGroups.Find(entity.ID);
            model.Name = entity.Name;
            model.Description = entity.Description;
            db.SaveChanges();
            return entity.ID;
        }
        public UserGroup GetByID(string id)
        {
            return db.UserGroups.Find(id);
        }
        public bool CheckIDExist(string id)
        {
            var model = db.UserGroups.Find(id);
            if (model == null)//không tồn tại
                return true;
            else return false;
        }
        public bool Delete(string id)
        {
            try
            {
                var UserGroup = db.UserGroups.Find(id);
                db.UserGroups.Remove(UserGroup);
                var credential = db.Credentials.Where(x => x.UserGroupID.Equals(id));
                foreach (var item in credential)
                {
                    db.Credentials.Remove(item);
                }
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool CheckUserIsUsed(string id)
        {
            var model = db.Users.FirstOrDefault(x => x.UserGroupID == id);
            if (model == null)
                return false;//khong tim thay
            else return true;//tim thay
        }
    }
}
