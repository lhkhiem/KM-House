using Common;
using Models.DAO;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace KMHouse.Areas.Admin.Controllers
{
    public class RoleController : BaseController
    {
        // Hiển thị danh sánh lên trang chủ
        [HasCredential(RoleID = "ROLE_VIEW")]
        public ActionResult Index()
        {
            @ViewBag.MenuActive = "mIndexRole";
            return View();
        }
        [HttpGet]
        public JsonResult LoadData(int type, string keyword, int pageIndex, int pageSize)
        {
            string str = NonUnicode.RemoveUnicode(keyword).ToLower();
            var model = new RoleDao().ListAll();
            switch (type)
            {
                case 0: model = model.Where(x => NonUnicode.RemoveUnicode(x.Name).ToLower().Contains(str)); break;
                case 1: model = model.Where(x => NonUnicode.RemoveUnicode(x.Name).ToLower().Contains(str)); break;
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
        public JsonResult SaveData(string strRole)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Role role = serializer.Deserialize<Role>(strRole);
            bool status = false;
            string action = string.Empty;
            string message = string.Empty;
            bool res = new RoleDao().CheckIDExist(role.ID);
            if (res)
            {
                var model = new RoleDao();
                try
                {
                    model.Insert(role);
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
                try
                {
                    var model = new RoleDao().Update(role);
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
        public JsonResult GetDetail(string id)
        {
            var model = new RoleDao().GetDetailById(id);
            return Json(new
            {
                data = model,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(string id)
        {
            var ckRole = new RoleDao().CheckRoleIsUsed(id);
            if (!ckRole)
            {
                var res = new RoleDao().Delete(id);
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
                    message="Role đã được dùng. Vui lòng kiểm tra lại!"
                }, JsonRequestBehavior.AllowGet);
            }
            

        }
    }
}