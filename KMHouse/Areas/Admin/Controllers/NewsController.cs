using Common;
using Models.DAO;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml.Linq;

namespace KMHouse.Areas.Admin.Controllers
{
    public class NewsController : BaseController
    {
        // Hiển thị danh sánh lên trang chủ
        [HasCredential(RoleID = "NEWS_VIEW")]
        public ActionResult Index()
        {
            @ViewBag.MenuActive = "mIndexNews";
            return View();
        }
        public JsonResult LoadData(int type, string keyword, bool status, int pageIndex, int pageSize)
        {
            string str = NonUnicode.RemoveUnicode(keyword).ToLower();
            var model = new NewsDao().ListAll();
            switch (type)
            {
                case 0:
                    model = model.Where(
                x => NonUnicode.RemoveUnicode(x.Name).ToLower().Contains(str)
            || NonUnicode.RemoveUnicode(x.NewsCategoryName).ToLower().Contains(str)
            || NonUnicode.RemoveUnicode(x.Tag).ToLower().Contains(str)
            ); break;
                case 1: model = model.Where(x => NonUnicode.RemoveUnicode(x.Name).ToLower().Contains(str)); break;
                case 2: model = model.Where(x => NonUnicode.RemoveUnicode(x.NewsCategoryName).ToLower().Contains(str)); break;
                case 3: model = model.Where(x => NonUnicode.RemoveUnicode(x.Tag).ToLower().Contains(str)); break;
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
        [HasCredential(RoleID = "NEWS_CREATE")]
        public ActionResult Create()
        {
            SetDropdownList();
            @ViewBag.MenuActive = "mIndexNews";
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(News entity)
        {
            entity.CreateBy = ((UserLoginSession)Session[ConstantSession.USER_SESSION]).UserName;
            var res = new NewsDao().Insert(entity);
            if (res)
            {
                ViewBag.Info = "Thêm mới thành công";
            }
            else
            {
                ViewBag.Info = "Thêm mới không thành công";
            }
            SetDropdownList();
            return View(entity);
        }
        [HasCredential(RoleID = "NEWS_EDIT")]
        public ActionResult Edit(long id)
        {
            ViewBag.MenuActive = "mIndexNews";
            var entity = new NewsDao().GetByID(id);
            SetDropdownList(entity.NewsCategoryID);
            return View(entity);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(News entity)
        {

            var res = new NewsDao().Update(entity);
            if (res)
            {
                ViewBag.Info = "Cập nhật thành công!";
            }
            else
            {
                ViewBag.Info = "Cập nhật không thành công!";
            }
            SetDropdownList(entity.NewsCategoryID);
            return View(entity);
        }
        public JsonResult ChangeStatus(long id)
        {
            var res = new NewsDao().ChangeStatus(id);
            return Json(new
            {
                status = res
            });
        }
        [HasCredential(RoleID = "NEWS_DELETE")]
        public JsonResult Delete(long id)
        {
            var res = new NewsDao().Delete(id);
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
        public void SetDropdownList(long? selectedProduct = null)
        {
            //set product category
            var model = new NewsCategoryDao().ListAll();
            List<NewsCategory> list = new List<NewsCategory>();
            model = model.OrderBy(x => x.DisplayOrder);
            foreach (var item in model)
            {
                if (item.ParentID == null)
                {
                    list.Add(item);
                    var child = model.Where(x => x.ParentID == item.ID);
                    foreach (var subitem in child)
                    {
                        subitem.Name = "-- " + subitem.Name;
                        list.Add(subitem);
                    }
                }
            }
            //Kết thúc sắp xếp.
            ViewBag.NewsCategoryID = new SelectList(list, "ID", "Name", selectedProduct);
        }
        public JsonResult ConvertString(string str)
        {
            string strConvert = StringHelper.ToUnsignString(str);
            return Json(new
            {
                str = strConvert
            }, JsonRequestBehavior.AllowGet);
        }
    }
}