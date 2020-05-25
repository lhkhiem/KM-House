using Models.DAO;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Common;

namespace KMHouse.Areas.Admin.Controllers
{
    public class UserGroupController : BaseController
    {
        [HasCredential(RoleID = "USERGROUP_VIEW")]
        public ActionResult Index()
        {
            ViewBag.MenuActive = "mIndexUserGroup";
            return View();
        }
        public JsonResult LoadData(int type, string keyword, int pageIndex, int pageSize)
        {
            string str = NonUnicode.RemoveUnicode(keyword).ToLower();
            var model = new UserGroupDao().ListAll();
            switch (type)
            {
                case 0: model = model.Where(x => NonUnicode.RemoveUnicode(x.Name.ToLower()).Contains(str) || x.ID.ToLower().Contains(keyword)); break;
                case 1: model = model.Where(x => NonUnicode.RemoveUnicode(x.Name.ToLower()).Contains(str)); break;
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
        [HttpPost]
        [HasCredential(RoleID = "USERGROUP_CREATE")]
        public JsonResult SaveData(string strUserGroup)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            UserGroup userGroup = serializer.Deserialize<UserGroup>(strUserGroup);
            bool status = false;
            string action = string.Empty;
            string message = string.Empty;
            bool res = new UserGroupDao().CheckIDExist(userGroup.ID);
            if (res)
            {
                var model = new UserGroupDao();
                try
                {
                    model.Insert(userGroup);
                    status = true;
                    action = "insert";
                }
                catch (Exception ex)
                {
                    status = false;
                    message = ex.Message;
                }
            }
            else
            {
                //update existing DB
                //save db
                try
                {
                    var model = new UserGroupDao().Update(userGroup);
                    status = true;
                    action = "update";
                }
                catch (Exception ex)
                {
                    status = false;
                    message = ex.Message;
                }

            }

            return Json(new
            {
                status = status,
                message = message,
                action = action
            });
        }
        [HasCredential(RoleID = "USERGROUP_EDIT")]
        public JsonResult GetDetail(string id)
        {
            var model = new UserGroupDao().GetByID(id);
            return Json(new
            {
                data = model,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        [HasCredential(RoleID = "USERGROUP_DELETE")]
        public JsonResult Delete(string id)
        {
            var isUsed = new UserGroupDao().CheckUserIsUsed(id);
            if (!isUsed)
            {
                var res = new UserGroupDao().Delete(id);
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
            else
            {
                return Json(new
                {
                    status = false,
                    message="Nhóm tài khoản đang dùng. Vui lòng kiểm tra lại"
                }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}