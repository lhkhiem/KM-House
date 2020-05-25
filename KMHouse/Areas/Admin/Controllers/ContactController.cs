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
    public class ContactController : BaseController
    {
        // Hiển thị danh sánh lên trang chủ
        [HasCredential(RoleID = "CONTACT_VIEW")]
        public ActionResult Index()
        {
            @ViewBag.MenuActive = "mIndexContact";
            return View();
        }
        public JsonResult LoadData(int type, string keyword, int pageIndex, int pageSize)
        {
            string str = NonUnicode.RemoveUnicode(keyword).ToLower();
            var model = new ContactDao().ListAll();
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
                totalCurent = totalRowCurent,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create()
        {
            @ViewBag.MenuActive = "mIndexContact";
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Contact contact)
        {
            var res = new ContactDao().Insert(contact);
            if (res)
            {
                ViewBag.Info = "Thêm mới thành công";
            }
            else
            {
                ViewBag.Info = "Thêm mới không thành công";
            }
            return View(contact);
        }
        public ActionResult Edit(int id)
        {
            ViewBag.MenuActive = "mIndexContact";
            var contact = new ContactDao().GetByID(id);
            return View(contact);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Contact contact)
        {
            var res = new ContactDao().Update(contact);
            if (res)
            {
                ViewBag.Info = "Cập nhật thành công!";
            }
            else
            {
                ViewBag.Info = "Cập nhật không thành công!";
            }
            return View();
        }
        public JsonResult ChangeStatus(long id)
        {
            var res = new ContactDao().ChangeStatus(id);
            return Json(new
            {
                status = res
            });
        }
        public JsonResult Delete(int id)
        {
            var res = new ContactDao().Delete(id);
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