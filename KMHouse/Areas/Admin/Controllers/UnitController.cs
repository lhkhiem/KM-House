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
    public class UnitController : BaseController
    {
        // Hiển thị danh sánh lên trang chủ
        [HasCredential(RoleID = "UNIT_VIEW")]
        public ActionResult Index()
        {
            @ViewBag.MenuActive = "mIndexUnit";
            return View();
        }
        [HttpGet]
        public JsonResult LoadData(int type, string keyword, int pageIndex, int pageSize)
        {
            string str = NonUnicode.RemoveUnicode(keyword).ToLower();
            var model = new UnitDao().ListAll();
            switch (type)
            {
                case 0: model = model.Where(x => NonUnicode.RemoveUnicode(x.Name).ToLower().Contains(str) || x.ID.ToString().Contains(keyword)); break;
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
                totalCurent= totalRowCurent,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveData(string strUnit)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Unit unit = serializer.Deserialize<Unit>(strUnit);
            bool status = false;
            string action = string.Empty;
            string message = string.Empty;
            //add new unit if id = 0
            if (unit.ID == 0)
            {
                var model = new UnitDao();
                try
                {
                    model.Insert(unit);
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
                    var model = new UnitDao().Update(unit);
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
        public JsonResult GetDetail(int id)
        {
            var model =new UnitDao().GetByID(id);
            return Json(new
            {
                data = model,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(int id)
        {
            byte status = 0;
            var ck = new UnitDao().CheckIsUsed(id);
            if (!ck)
            {
                var res = new UnitDao().Delete(id);
                if (res)
                {
                    status = 1;//xóa
                }
                else
                {
                    status = 0;//không xóa được
                }
            }
            else
                status = 2;//đang được sử dụng
            return Json(new
            {
                status = status
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
