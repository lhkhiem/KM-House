using Common;
using Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KMHouse.Areas.Admin.Controllers
{
    public class FeedbackController : Controller
    {
        [HasCredential(RoleID = "FEEDBACK_VIEW")]
        public ActionResult Index()
        {
            @ViewBag.MenuActive = "mIndexContact";
            return View();
        }
        public JsonResult LoadData(int type, string keyword, int pageIndex, int pageSize)
        {
            string str = NonUnicode.RemoveUnicode(keyword).ToLower();
            var model = new FeedbackDao().ListAll();
            switch (type)
            {
                case 0: model = model.Where(x => NonUnicode.RemoveUnicode(x.Name).ToLower().Contains(str) || x.Email.Contains(keyword)); break;
                case 1: model = model.Where(x => NonUnicode.RemoveUnicode(x.Name).ToLower().Contains(str)); break;
                case 2: model = model.Where(x => NonUnicode.RemoveUnicode(x.Email).ToLower().Contains(str)); break;
            }

            int totalRow = model.Count();
            model = model.OrderByDescending(x => x.CreateDate)
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
        public JsonResult Delete(int id)
        {
            var res = new FeedbackDao().Delete(id);
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
            var res = new FeedbackDao().ChangeStatus(id);
            return Json(new
            {
                status = res
            });
        }
        public JsonResult ShowContent(int id)
        {
            var res = new FeedbackDao().GetById(id);
            return Json(new
            {
                data = res,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
    }
}