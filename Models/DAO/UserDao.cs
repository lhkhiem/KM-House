using Models.EF;
using Models.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Models.DAO
{
    public class UserDao
    {
        DBContext db = null;
        public UserDao()
        {
            db = new DBContext();
        }
        public IEnumerable<UserViewModel> ListAll()
        {
            var model = from a in db.Users
                        join b in db.UserGroups
                        on a.UserGroupID equals b.ID
                        select new UserViewModel()
                        {
                            ID = a.ID,
                            UserName = a.UserName,
                            Name = a.Name,
                            Email = a.Email,
                            Phone = a.Phone,
                            Address=a.Address,
                            CreateDate = a.CreateDate,
                            UserGroup = b.Name,
                            Status = a.Status
                        };
            return model.ToList();
        }
        public long Insert(UserViewModel entity)
        {
            var model = new User();
            model.Name = entity.Name;
            model.UserName = entity.UserName;
            model.Password = entity.Password;
            model.Phone = entity.Phone;
            model.CreateBy = entity.CreateBy;
            model.CreateDate = DateTime.Now;
            model.Address = entity.Address;
            model.Email = entity.Email;
            model.Status = entity.Status;
            model.UserGroupID = entity.UserGroupID;
            db.Users.Add(model);
            db.SaveChanges();
            return model.ID;
        }
        public bool Update(UserViewModel entity)
        {
            var model = db.Users.Find(entity.ID);
            model.Name = entity.Name;
            model.UserName = entity.UserName;
            model.Password = entity.Password;
            model.Phone = entity.Phone;
            model.ModifiedBy = entity.ModifiedBy;
            model.ModifiedDate = DateTime.Now;
            model.Address = entity.Address;
            model.Email = entity.Email;
            model.Status = entity.Status;
            model.UserGroupID = entity.UserGroupID;
            db.SaveChanges();
            return true;
        }
        public bool UpdatePassword(long id,string password)
        {
            var model = db.Users.Find(id);
            model.Password = password;
            db.SaveChanges();
            return true;
        }
        public bool Delete(int id)
        {
            try
            {
                var User = db.Users.Find(id);
                db.Users.Remove(User);
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
            var user = db.Users.Find(id);
            user.Status = !user.Status;
            db.SaveChanges();
            return user.Status;
        }
        public bool CheckUserExist(string userName)
        {
            return db.Users.Count(x => x.UserName == userName) > 0;
        }
        public bool CheckEmailExist(string email)
        {
            return db.Users.Count(x => x.Email == email) > 0;
        }
        public User GetUserExist(string userName)
        {
            return db.Users.SingleOrDefault(x => x.UserName == userName);
        }
        public UserViewModel GetDetailById(long id)
        {
            var model= db.Users.Find(id);
            var user = new UserViewModel();

            user.Name = model.Name;
            user.UserName = model.UserName;
            user.Password = "";
            user.Phone = model.Phone;
            user.Address = model.Address;
            user.Email = model.Email;
            user.Status = model.Status;
            user.UserGroupID = model.UserGroupID;
            return user;
        }
        public UserViewModel GetDetailByEmail(string email)
        {
            var model = db.Users.FirstOrDefault(x=>x.Email==email);
            var user = new UserViewModel();
            user.ID = model.ID;
            user.Name = model.Name;
            user.UserName = model.UserName;
            user.Password = model.Password;
            user.Phone = model.Phone;
            user.Address = model.Address;
            user.Email = model.Email;
            user.Status = model.Status;
            user.UserGroupID = model.UserGroupID;
            return user;
        }
        public List<string> GetListCredential(string userName)
        {
            var user = db.Users.Single(x => x.UserName == userName);
            var data = (from a in db.Credentials
                        join b in db.UserGroups on a.UserGroupID equals b.ID
                        join c in db.Roles on a.RoleID equals c.ID
                        where b.ID == user.UserGroupID
                        select new
                        {
                            RoleID = a.RoleID,
                            UserGroupID = a.UserGroupID
                        }).AsEnumerable().Select(x => new Credential()
                        {
                            RoleID = x.RoleID,
                            UserGroupID = x.UserGroupID
                        });
            return data.Select(x => x.RoleID).ToList();

        }
        public int Login(string userName, string passWord, bool isLoginAdmin = false)
        {
            var result = db.Users.SingleOrDefault(x => x.UserName == userName);
            
            if (result == null)
            {
                return 0;//không tìm thấy user
            }
            else
            {//tim thay user
                if (isLoginAdmin == true)
                {
                    if (result.UserGroupID == Constants.ADMIN_GROUP || result.UserGroupID == Constants.MOD_GROUP)
                    {
                        if (result.Status == false)
                        {
                            return -1;
                        }
                        else
                        {
                            if (result.Password == passWord)
                                return 1;
                            else
                                return -2;
                        }
                    }
                    else
                    {
                        return -3;
                    }
                }
                else
                {
                    if (result.Status == false)
                    {
                        return -1;
                    }
                    else
                    {
                        if (result.Password == passWord)
                            return 1;
                        else
                            return -2;
                    }
                }
            }

        }
        public int LoginByEmail(string email, string passWord, bool isLoginAdmin = false)
        {
            var result = db.Users.SingleOrDefault(x => x.Email == email);

            if (result == null)
            {
                return 0;//không tìm thấy user
            }
            else
            {//tim thay user
                if (isLoginAdmin == true)
                {
                    if (result.UserGroupID == Constants.ADMIN_GROUP || result.UserGroupID == Constants.MOD_GROUP)
                    {
                        if (result.Status == false)
                        {
                            return -1;
                        }
                        else
                        {
                            if (result.Password == passWord)
                                return 1;
                            else
                                return -2;
                        }
                    }
                    else
                    {
                        return -3;
                    }
                }
                else
                {
                    if (result.Status == false)
                    {
                        return -1;
                    }
                    else
                    {
                        if (result.Password == passWord)
                            return 1;
                        else
                            return -2;
                    }
                }
            }

        }
    }
}
