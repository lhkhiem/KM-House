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
    public class ProductController : BaseController
    {
        // Hiển thị danh sánh lên trang chủ
        [HasCredential(RoleID = "PRODUCT_VIEW")]
        public ActionResult Index()
        {
            @ViewBag.MenuActive = "mIndexProduct";
            return View();
        }
        public JsonResult LoadData(int type, string keyword, bool status, int pageIndex, int pageSize)
        {
            string str = NonUnicode.RemoveUnicode(keyword).ToLower();
            var model = new ProductDao().ListAll();
            switch (type)
            {
                case 0:
                    model = model.Where(
                x => NonUnicode.RemoveUnicode(x.Name).ToLower().Contains(str)
            || NonUnicode.RemoveUnicode(x.ProductCategoryName).ToLower().Contains(str)
            || NonUnicode.RemoveUnicode(x.UnitName).ToLower().Contains(str)
            ); break;
                case 1: model = model.Where(x => NonUnicode.RemoveUnicode(x.Name).ToLower().Contains(str)); break;
                case 2: model = model.Where(x => NonUnicode.RemoveUnicode(x.ProductCategoryName).ToLower().Contains(str)); break;
                case 3: model = model.Where(x => NonUnicode.RemoveUnicode(x.UnitName).ToLower().Contains(str)); break;
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
        public ActionResult Create()
        {
            SetDropdownList();
            @ViewBag.MenuActive = "mIndexProduct";
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Product product)
        {
            product.CreateBy = ((UserLoginSession)Session[ConstantSession.USER_SESSION]).UserName;
            product.Price = product.Price ==null ? 0 : product.Price;
            var res = new ProductDao().Insert(product);
            if (res)
            {
                ViewBag.Info = "Thêm mới thành công";
            }
            else
            {
                ViewBag.Info = "Thêm mới không thành công";
            }
            SetDropdownList();
            return RedirectToAction("Index");
        }
        public ActionResult Edit(long id)
        {

            ViewBag.MenuActive = "mIndexProduct";
            var product = new ProductDao().GetByID(id);
            SetDropdownList(product.UnitID, product.ProductCategoryID);
            return View(product);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Product product)
        {

            var res = new ProductDao().Update(product);
            if (res)
            {
                ViewBag.Info = "Cập nhật thành công!";
            }
            else
            {
                ViewBag.Info = "Cập nhật không thành công!";
            }
            SetDropdownList(product.UnitID, product.ProductCategoryID);
            return RedirectToAction("Index");
        }
        public JsonResult ChangeStatus(long id)
        {
            var res = new ProductDao().ChangeStatus(id);
            return Json(new
            {
                status = res
            });
        }
        public JsonResult Delete(long id)
        {
            var res = new ProductDao().Delete(id);
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
        public void SetDropdownList(int? selectedUnit = null, long? selectedProduct = null)
        {
            //set product category
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
            ViewBag.ProductCategoryID = new SelectList(list, "ID", "Name", selectedProduct);

            //set unit
            var unit = new UnitDao();
            ViewBag.UnitID = new SelectList(unit.ListAll(), "ID", "Name", selectedUnit);
        }

        public JsonResult LoadImages(long id)
        {
            ProductDao dao = new ProductDao();
            var product = dao.GetByID(id);
            var images = product.MoreImage;
            List<string> listImagesReturn = new List<string>();
            if (images != null)
            {
                XElement xImages = XElement.Parse(images);
                foreach (XElement element in xImages.Elements())
                {
                    listImagesReturn.Add(element.Value);
                }
                return Json(new
                {
                    data = listImagesReturn
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    data = listImagesReturn
                }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult SaveImages(long id, string images)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var listImages = serializer.Deserialize<List<string>>(images);
            XElement xElement = new XElement("Images");
            foreach (var item in listImages)
            {
                var subStringItem = item.Substring(22);

                xElement.Add(new XElement("Images", subStringItem));
            }
            var dao = new ProductDao();
            try
            {
                dao.UpdateImages(id, xElement.ToString());
                return Json(new
                {
                    status = true
                });
            }
            catch (Exception )
            {
                return Json(new
                {
                    status = false
                });
            }

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