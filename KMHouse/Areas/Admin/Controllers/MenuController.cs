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
    public class MenuController : BaseController
    {
        [HasCredential(RoleID = "MENU_VIEW")]
        public ActionResult Index()
        {
            SetDropdownList();
            ViewBag.MenuActive = "mIndexMenu";
            return View();
        }
        public JsonResult LoadData(int menuType, int type, string keyword, int pageIndex, int pageSize)
        {
            string str = NonUnicode.RemoveUnicode(keyword).ToLower();
            var model = new MenuDao().ListAll();
            switch (type)
            {
                case 0: model = model.Where(x => NonUnicode.RemoveUnicode(x.Text.ToLower()).Contains(str)||x.Link.Contains(str)); break;
                case 1: model = model.Where(x => NonUnicode.RemoveUnicode(x.Text.ToLower()).Contains(str)); break;
            }
            model = model.Where(x => x.MenuTypeID == menuType);
            int totalRow = model.Count();
            model = model.OrderBy(x => x.DisplayOrder)
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
        [HasCredential(RoleID = "MENU_CREATE")]
        public JsonResult SaveData(string strMenu)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Menu menu = serializer.Deserialize<Menu>(strMenu);
            bool status = false;
            string action = string.Empty;
            string message = string.Empty;
            bool res = new MenuDao().CheckIDExist(menu.ID);
            if (res)
            {
                var model = new MenuDao();
                try
                {
                    model.Insert(menu);
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
                    var model = new MenuDao().Update(menu);
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
        [HasCredential(RoleID = "MENU_EDIT")]
        public JsonResult GetDetail(int id)
        {
            var model = new MenuDao().GetByID(id);
            return Json(new
            {
                data = model,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        [HasCredential(RoleID = "MENU_DELETE")]
        public JsonResult Delete(int id)
        {
            var res = new MenuDao().Delete(id);
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
        public JsonResult ChangeStatus(int id)
        {
            var res = new MenuDao().ChangeStatus(id);
            return Json(new
            {
                status = res
            });
        }
        public JsonResult ChangeOrder(int id, int order, int menuType)
        {
            var res = new MenuDao().ChangeOrder(id,order, menuType);
            if (res)
            {
                return Json(new
                {
                    status = true
                });
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }
            
        }
        public void SetDropdownList(string selectedId = null)
        {
            var dao = new MenuTypeDao();
            ViewBag.MenuTypeID = new SelectList(dao.ListAll(), "ID", "Name", selectedId);
        }
    }
}