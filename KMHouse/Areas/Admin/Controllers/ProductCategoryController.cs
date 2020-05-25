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
    public class ProductCategoryController : BaseController
    {
        [HasCredential(RoleID = "PRODUCTCATEGORY_VIEW")]
        public ActionResult Index()
        {
            SetDropdownList();
            ViewBag.MenuActive = "mIndexProductCategory";
            return View();
        }
        public JsonResult LoadData(int type, string keyword, int pageIndex, int pageSize)
        {
            string str = NonUnicode.RemoveUnicode(keyword).ToLower();
            var model = new ProductCategoryDao().ListAll();
            switch (type)
            {
                case 0: model = model.Where(x => NonUnicode.RemoveUnicode(x.Name.ToLower()).Contains(str)); break;
            }

            int totalRow = model.Count();
            //Sắp xếp theo danh mục cha
            var list = new List<ProductCategory>();
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
        [HasCredential(RoleID = "PRODUCTCATEGORY_CREATE")]
        public JsonResult SaveData(string strProductCategory)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            ProductCategory productCategory = serializer.Deserialize<ProductCategory>(strProductCategory);
            bool status = false;
            string action = string.Empty;
            string message = string.Empty;
            bool res = new ProductCategoryDao().CheckIDExist(productCategory.ID);
            if (res)
            {
                var model = new ProductCategoryDao();
                try
                {
                    if (productCategory.ParentID==null)
                    {
                        productCategory.CreateBy = ((UserLoginSession)Session[ConstantSession.USER_SESSION]).UserName;
                        model.Insert(productCategory);
                        status = true;
                        action = "insert";
                    }
                    else
                    {
                        bool haveParent = new ProductCategoryDao().HasChild(productCategory.ParentID);
                        if (!haveParent)
                        {
                            productCategory.CreateBy = ((UserLoginSession)Session[ConstantSession.USER_SESSION]).UserName;
                            model.Insert(productCategory);
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
                    var model = new ProductCategoryDao().Update(productCategory);
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
        [HasCredential(RoleID = "PRODUCTCATEGORY_EDIT")]
        public JsonResult GetDetail(long id)
        {
            var model = new ProductCategoryDao().GetByID(id);
            return Json(new
            {
                data = model,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        [HasCredential(RoleID = "PRODUCTCATEGORY_DELETE")]
        public JsonResult Delete(long id)
        {
            bool status=false;
            string message="";
            var isUsed = new ProductCategoryDao().CheckProductIsUsed(id);
            var child = new ProductCategoryDao().GetChild(id);
            if (!isUsed)
            {
                if (!child)
                {
                    var res = new ProductCategoryDao().Delete(id);

                    if (res)
                    {
                        status = true;
                    }
                    else
                    {
                        status = false;
                        message = "Có lỗi xảy ra!";
                    }
                }
                else
                {
                    status = false;
                    message = "Vui lòng xóa hết danh mục con trước khi xóa danh mục này!";
                }
            }
            else
            {
                status = false;
                message = "Có Product đang dùng. Vui lòng kiểm tra lại!";
            }
            return Json(new
            {
                status = status,
                message = message
            }, JsonRequestBehavior.AllowGet);
        }
        public void SetDropdownList(string selectedId = null)
        {
            var model = new ProductCategoryDao().ListAll();
            List<ProductCategory> list = new List<ProductCategory>();
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
            var res = new ProductCategoryDao().ChangeOrder(id, order);
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