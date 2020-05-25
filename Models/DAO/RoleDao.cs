using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class RoleDao
    {
        DBContext db = null;
        public RoleDao()
        {
            db = new DBContext();
        }
        public IEnumerable<Role> ListAll()
        {
            return db.Roles.ToList();
        }
        public bool Insert(Role entity)
        {
            db.Roles.Add(entity);
            db.SaveChanges();
            return true;
        }
        public bool Update(Role entity)
        {
            var model = db.Roles.Find(entity.ID);
            model.Name = entity.Name;
            db.SaveChanges();
            return true;
        }
        public Role GetDetailById(string id)
        {
            return db.Roles.Find(id);
        }
        public bool Delete(string id)
        {
            try
            {
                var role = db.Roles.Find(id);
                db.Roles.Remove(role);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool CheckIDExist(string id)
        {
            var model = db.Roles.Find(id);
            if (model == null)//không tồn tại
                return true;
            else return false;
        }
        //Kiểm tra Role có được dùng để phân quyền chưa?có return true/ không return false
        public bool CheckRoleIsUsed(string id)
        {
            var model = db.Credentials.FirstOrDefault(x => x.RoleID.Equals(id));
            if (model == null)
                return false;//khong tim thay
            else return true;//tim thay
        }
    }
}
