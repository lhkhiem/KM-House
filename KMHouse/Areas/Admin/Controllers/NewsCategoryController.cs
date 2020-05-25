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
    public class NewsCategoryController : BaseController
    {
        [HasCredential(RoleID = "NEWSCATEGORY_VIEW")]
        public ActionResult Index()
        {
            SetDropdownList();
            ViewBag.MenuActive = "mIndexNewsCategory";
            return View();
        }
        public JsonResult LoadData(int type, string keyword, int pageIndex, int pageSize)
        {
            string str = NonUnicode.RemoveUnicode(keyword).ToLower();
            var model = new NewsCategoryDao().ListAll();
            switch (type)
            {
                case 0: model = model.Where(x => NonUnicode.RemoveUnicode(x.Name.ToLower()).Contains(str)); break;
            }

            int totalRow = model.Count();
            //Sắp xếp theo danh mục cha
            var list = new List<NewsCategory>();
            model = model.OrderBy(x => x.DisplayOrder);
            foreach (var item in model)
            {
                if (item.ParentID == null)
                {
                    list.Add(item);
                    var child = model.Where(x => x.ParentID == item.ID);
                    foreach (var subitem in child)
                    {
                        list.Add(subitem);
                    }
                }
            }
            ////Kết thúc sắp xếp.

            model = list
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
        [HasCredential(RoleID = "NEWSCATEGORY_CREATE")]
        public JsonResult SaveData(string strNewsCategory)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            NewsCategory newsCategory = serializer.Deserialize<NewsCategory>(strNewsCategory);
            bool status = false;
            string action = string.Empty;
            string message = string.Empty;
            bool res = new NewsCategoryDao().CheckIDExist(newsCategory.ID);
            if (res)
            {
                var model = new NewsCategoryDao();
                try
                {
                    if (newsCategory.ParentID==null)
                    {
                        newsCategory.CreateBy = ((UserLoginSession)Session[ConstantSession.USER_SESSION]).UserName;
                        model.Insert(newsCategory);
                        status = true;
                        action = "insert";
                    }
                    else
                    {
                        bool haveParent = new NewsCategoryDao().IsSubMenu(newsCategory.ParentID);
                        if (!haveParent)
                        {
                            newsCategory.CreateBy = ((UserLoginSession)Session[ConstantSession.USER_SESSION]).UserName;
                            model.Insert(newsCategory);
                            status = true;
                            action = "insert";
                        }
                    }
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
                    var model = new NewsCategoryDao().Update(newsCategory);
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
        [HasCredential(RoleID = "NEWSCATEGORY_EDIT")]
        public JsonResult GetDetail(long id)
        {
            var model = new NewsCategoryDao().GetByID(id);
            return Json(new
            {
                data = model,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        [HasCredential(RoleID = "NEWSCATEGORY_DELETE")]
        public JsonResult Delete(long id)
        {
            var isUsed = new NewsCategoryDao().CheckNewsIsUsed(id);
            if (!isUsed)
            {
                var res = new NewsCategoryDao().Delete(id);
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
                    message = "Có tin đang dùng. Vui lòng kiểm tra lại"
                }, JsonRequestBehavior.AllowGet);
            }

        }
        public void SetDropdownList(string selectedId = null)
        {
            var model = new NewsCategoryDao().ListAll();
            List<NewsCategory> list = new List<NewsCategory>();
            model = model.OrderBy(x => x.ID);
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
            ViewBag.ID = new SelectList(list, "ID", "Name", selectedId);
        }
        public JsonResult ConvertString(string str)
        {
            string strConvert = StringHelper.ToUnsignString(str);
            return Json(new
            {
                str = strConvert
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ChangeOrder(int id, int order)
        {
            var res = new NewsCategoryDao().ChangeOrder(id, order);
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
    }
}