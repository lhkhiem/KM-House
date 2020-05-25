using Common;
using Models.DAO;
using Models.EF;
using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace KMHouse.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        [HasCredential(RoleID = "USER_VIEW")]
        public ActionResult Index()
        {
            @ViewBag.MenuActive = "mIndexUser";
            return View();
        }
        public JsonResult LoadData(int type, string keyword, bool status, int pageIndex, int pageSize)
        {
            string str = NonUnicode.RemoveUnicode(keyword).ToLower();
            var model = new UserDao().ListAll();
            switch (type)
            {
                case 0:
                    model = model.Where(x => NonUnicode.RemoveUnicode(x.Name.ToLower()).Contains(str)
            || x.Phone.ToString().Contains(str)
            || NonUnicode.RemoveUnicode(x.UserGroup.ToLower()).Contains(str)
            ); break;
                case 1: model = model.Where(x => NonUnicode.RemoveUnicode(x.Name.ToLower()).Contains(str)); break;
                case 2: model = model.Where(x => NonUnicode.RemoveUnicode(x.UserGroup.ToLower()).Contains(str)); break;
                case 3: model = model.Where(x => x.Phone.ToString().Contains(str)); break;
            }
            if (status)
            {
                model = model.Where(x => x.Status == true);
            }
            else
            {
                model = model.Where(x => x.Status == false);
            }
            int totalRow = model.Count();
            model = model.OrderByDescending(x => x.ID)
              .Skip((pageIndex - 1) * pageSize)
              .Take(pageSize);
            int totalRowCurent = model.Count();

            return Json(new
            {
                data = model,
                total = totalRow,
                totalCurent = totalRowCurent,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        [HasCredential(RoleID = "USER_CREATE")]
        public ActionResult Create()
        {
            SetDropdownList();
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var res = dao.CheckUserExist(user.UserName);
                if (!res)
                {
                    var passEncryp = Encryptor.MD5Hash(Encryptor.EncodeTo64(user.Password));
                    user.Password = passEncryp;
                    user.CreateBy = ((UserLoginSession)Session[ConstantSession.USER_SESSION]).UserName;
                    long id = dao.Insert(user);
                    if (id > 0)
                    {
                        ViewBag.info = "Thêm mới thành công!";
                    }
                    else
                    {
                        ModelState.AddModelError("", "Thêm mới không thành công!");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Tài khoản đã tồn tại!");
                }
            }
            SetDropdownList();
            return View(user);
        }
        [HasCredential(RoleID = "USER_EDIT")]
        public ActionResult Edit(int id)
        {
            var user = new UserDao().GetDetailById(id);
            SetDropdownList(user.UserGroupID);
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var passEncryp = Encryptor.MD5Hash(Encryptor.EncodeTo64(user.Password));
                user.Password = passEncryp;
                user.ModifiedBy = ((UserLoginSession)Session[ConstantSession.USER_SESSION]).UserName;
                var res = dao.Update(user);
                if (res)
                {
                    ViewBag.info = "Cập nhật thành công!";
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật không thành công!");
                }
            }
            SetDropdownList(user.UserGroupID);
            return View();
        }
        [HasCredential(RoleID = "USER_DELETE")]
        public JsonResult Delete(int id)
        {
            var res = new UserDao().Delete(id);
            if (res)
            {
                return Json(new
                {
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    status = false
                }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var res = new UserDao().ChangeStatus(id);
            return Json(new
            {
                status = res
            });
        }
        public void SetDropdownList(string selectedId = null)
        {
            var dao = new UserGroupDao();
            ViewBag.UserGroupID = new SelectList(dao.ListAll(), "ID", "Name", selectedId);
        }
    }
}