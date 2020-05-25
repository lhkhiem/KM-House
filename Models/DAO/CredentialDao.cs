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
    public class CredentialDao
    {
        DBContext db = null;
        public CredentialDao()
        {
            db = new DBContext();
        }
        public IEnumerable<CredentialViewModel> ListByUserGroup(string userGroup)
        {
            var model = from a in db.Credentials
                        join b in db.UserGroups on a.UserGroupID equals b.ID
                        join c in db.Roles on a.RoleID equals c.ID
                        select new CredentialViewModel()
                        {
                            UserGroupID=a.UserGroupID,
                            RoleID=a.RoleID,
                            UserGroupName=b.Name,
                            RoleName=c.Name
                        };
            return model.Where(x=>x.UserGroupID.Equals(userGroup)).ToList();
        }
        public bool Insert(Credential entity)
        {
            try
            {
                db.Credentials.Add(entity);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
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
        public IEnumerable<Role> CheckRoleExist(string userGruopID)
        {
            var model =
                (from a in db.Roles
                 select new
                 {
                     ID = a.ID,
                     Name = a.Name
                 }).Except
                    (from b in db.Credentials
                        join c in db.Roles on b.RoleID equals c.ID
                        where b.UserGroupID.Equals(userGruopID)
                        select new
                        {
                            ID = b.RoleID,
                            Name = c.Name
                        }
                    ).AsEnumerable().Select(x => new Role()
                    {
                        ID = x.ID,
                        Name = x.Name
                    });
            return model.ToList();
        }
        public bool Delete(string userGroupId, string roleId)
        {
            try
            {
                var credential = db.Credentials.FirstOrDefault(x=>x.UserGroupID.Equals(userGroupId) && x.RoleID.Equals(roleId));
                db.Credentials.Remove(credential);
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
