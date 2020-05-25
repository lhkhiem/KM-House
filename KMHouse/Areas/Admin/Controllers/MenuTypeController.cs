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
    public class MenuTypeController : BaseController
    {
        [HasCredential(RoleID = "MENUTYPE_VIEW")]
        public ActionResult Index()
        {
            ViewBag.MenuActive = "mIndexMenuType";
            return View();
        }
        public JsonResult LoadData(int type, string keyword, int pageIndex, int pageSize)
        {
            string str = NonUnicode.RemoveUnicode(keyword).ToLower();
            var model = new MenuTypeDao().ListAll();
            switch (type)
            {
                case 0: model = model.Where(x => NonUnicode.RemoveUnicode(x.Name.ToLower()).Contains(str)); break;
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
        public JsonResult SaveData(string strMenuType)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            MenuType menuType = serializer.Deserialize<MenuType>(strMenuType);
            bool status = false;
            string action = string.Empty;
            string message = string.Empty;
            bool res = new MenuTypeDao().CheckIDExist(menuType.ID);
            if (res)
            {
                var model = new MenuTypeDao();
                try
                {
                    model.Insert(menuType);
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
                    var model = new MenuTypeDao().Update(menuType);
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
        [HasCredential(RoleID = "MENUTYPE_EDIT")]
        public JsonResult GetDetail(int id)
        {
            var model = new MenuTypeDao().GetByID(id);
            return Json(new
            {
                data = model,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        [HasCredential(RoleID = "MENUTYPE_DELETE")]
        public JsonResult Delete(int id)
        {
            var isUsed = new MenuTypeDao().CheckMenuIsUsed(id);
            if (!isUsed)
            {
                var res = new MenuTypeDao().Delete(id);
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
                    message="Có menu đang dùng. Vui lòng kiểm tra lại"
                }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}