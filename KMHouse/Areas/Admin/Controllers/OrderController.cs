using Common;
using Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KMHouse.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        [HasCredential(RoleID = "ORDER_VIEW")]
        public ActionResult Index()
        {
            @ViewBag.MenuActive = "mIndexOrder";
            return View();
        }
        public JsonResult LoadData(int type, string keyword, int status, int pageIndex, int pageSize)
        {
            string str = NonUnicode.RemoveUnicode(keyword).ToLower();
            var model = new OrderDao().ListAll();
            switch (type)
            {
                case 0:
                    model = model.Where(x => NonUnicode.RemoveUnicode(x.NameAccount.ToLower()).Contains(str)
            || x.ShipMobile.ToString().Contains(str)
            || NonUnicode.RemoveUnicode(x.ShipName.ToLower()).Contains(str)
            || NonUnicode.RemoveUnicode(x.ShipAddress.ToLower()).Contains(str)
            ); break;
                case 1: model = model.Where(x => NonUnicode.RemoveUnicode(x.NameAccount.ToLower()).Contains(str)); break;
                case 2: model = model.Where(x => NonUnicode.RemoveUnicode(x.ShipName.ToLower()).Contains(str)); break;
                case 3: model = model.Where(x => x.ShipMobile.ToString().Contains(str)); break;
                case 4: model = model.Where(x => NonUnicode.RemoveUnicode(x.ShipAddress.ToLower()).Contains(str)); break;
            }
            switch (status)
            {
                case 0: break;//tất cả
                case 1: model = model.Where(x => x.Status == 1); break;//đơn mới
                case 2: model = model.Where(x => x.Status == 2); break;//đang giao
                case 3: model = model.Where(x => x.Status == 3); break;//đã giao
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
        public JsonResult ChangeStatus(long id)
        {
            var res = new OrderDao().ChangeStatus(id);
            return Json(new
            {
                status = res
            });
        }
        public ActionResult Detail(long id)
        {
            var orderDetail = new OrderDetailDao().OrderDetailListAll().Where(x => x.OrderID == id).ToList();
            return View(orderDetail);
        }
        public JsonResult Delete(long id)
        {
            var res = new OrderDao().Delete(id);
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
    }
}