using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Common;
using Models.DAO;
using Models.EF;
using Models.ViewModels;

namespace KMHouse.Controllers
{
    public class ProductHomeController : Controller
    {
        public List<string> LoadImages(long id)
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
                return listImagesReturn;
            }
            else
            {
                return listImagesReturn;
            }

        }
        public ActionResult ProductDetail(long id)
        {
            var product = new ProductDao().GetByIDView(id);
            ViewBag.ListImage = this.LoadImages(id);
            ViewBag.Category = new ProductCategoryDao().GetByID(product.ProductCategoryID);
            ViewBag.RelateProducts = new ProductDao().ListRelateProduct(id);
            ViewBag.Tag = new ProductDao().ListTag(id);
            return View(product);
        }
        public ActionResult ProductTag(string tagId, int pageIndex = 1, int pageSize = 6, string sort = "newest")
        {
            ViewBag.Tag = new ProductDao().GetTag(tagId);
            ViewBag.CategoryList = new ProductCategoryDao().ListAll();
            if (sort == "newest") ViewBag.Newest = "selected";
            if (sort == "price-up") ViewBag.PriceUp = "selected";
            if (sort == "price-down") ViewBag.PriceDown = "selected";
            if (sort == "salsest") ViewBag.Salsest = "selected";
            ViewBag.Sort = sort;
            //Lấy danh sách sản phẩm thỏa điều kiện là danh mục sản phẩm
            var model = new ProductDao().ListAllByTag(tagId);
            
            //Lấy tổng các record sản phẩm thỏa điều kiện
            int totalRecord = model.Count();
            //sắp xếp
            switch (sort)
            {
                case "newest": model = model.OrderByDescending(x => x.CreateDate).ToList(); break;
                case "price-up": model = model.OrderBy(x => (x.PromotionPrice != null && x.PromotionPrice > 0) ? x.PromotionPrice : x.Price).ToList(); break;
                case "price-down": model = model.OrderByDescending(x => (x.PromotionPrice != null && x.PromotionPrice > 0) ? x.PromotionPrice : x.Price).ToList(); break;
                case "salest": model = model.OrderByDescending(x => (double)((double)x.PromotionPrice / (double)x.Price)).ToList(); break;
            }
            //Lấy 1 khoảng trong list sản phẩm từ pageIndex đến pageSize để phân trang
            model = model.Skip((pageIndex - 1) * pageSize)
              .Take(pageSize).ToList();
            //Đếm lấy số record thực tế ở đoạn record hiện tại
            int totalRowCurent = model.Count();
            //Các biến để phân trang
            ViewBag.totalRecord = totalRecord;
            ViewBag.pageIndex = pageIndex;
            int recordStartPosition = ((pageIndex - 1) * pageSize) + 1;
            int recordEndPosition = ((recordStartPosition - 1 + pageSize) < totalRecord) ? (recordStartPosition - 1 + pageSize) : recordStartPosition - 1 + totalRowCurent;
            ViewBag.recordStartPosition = recordStartPosition;
            ViewBag.recordEndPosition = recordEndPosition;
            int maxPage = 5;
            int totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize));
            if (totalRecord % pageSize > 0 && totalRecord > pageSize) totalPage += 1;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = pageIndex + 1;
            ViewBag.Prev = pageIndex - 1;
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            return View(model);
        }
        public ActionResult ProductOfCategory(long cateId, int pageIndex = 1, int pageSize = 6, string sort = "newest")
        {
            //Lấy danh sách danh mục sản phẩm
            ViewBag.Category = new ProductCategoryDao().GetByID(cateId);
            ViewBag.ListCategoryWithout = new ProductCategoryDao().ListAll().Where(x => x.ID != cateId).ToList();
            ViewBag.ListCategoryChild = new ProductCategoryDao().ListAll().Where(x => x.ParentID == cateId).ToList();
            if (sort == "newest") ViewBag.Newest = "selected";
            if (sort == "price-up") ViewBag.PriceUp = "selected";
            if (sort == "price-down") ViewBag.PriceDown = "selected";
            if (sort == "salsest") ViewBag.Salsest = "selected";
            ViewBag.Sort = sort;
            //Lấy danh sách sản phẩm thỏa điều kiện là danh mục sản phẩm
            var model = new ProductDao().ListAll();
            //Tạo list trống
            List<ProductViewModel> list = new List<ProductViewModel>();
            //Tìm tất cả danh mục con
            var listCateChild = new ProductCategoryDao().ListAll().Where(x => x.ParentID == cateId);

            if (listCateChild.Count() > 0)
            {
                foreach (var item in listCateChild)
                {
                    foreach (var product in model)
                    {
                        if (product.ProductCategoryID == item.ID)
                        {
                            list.Add(product);
                        }
                    }
                }
            }
            else
            {
                list = model.Where(x => x.ProductCategoryID == cateId).ToList();
            }
            //Lấy tổng các record sản phẩm thỏa điều kiện
            int totalRecord = list.Count();
            //sắp xếp
            switch (sort)
            {
                case "newest": list = list.OrderByDescending(x => x.CreateDate).ToList(); break;
                case "price-up": list = list.OrderBy(x => (x.PromotionPrice != null&& x.PromotionPrice>0) ? x.PromotionPrice : x.Price).ToList(); break;
                case "price-down": list = list.OrderByDescending(x => (x.PromotionPrice != null && x.PromotionPrice > 0) ? x.PromotionPrice : x.Price).ToList(); break;
                case "salest": list = list.OrderByDescending(x => (double)((double)x.PromotionPrice / (double)x.Price)).ToList(); break;
            }
            //Lấy 1 khoảng trong list sản phẩm từ pageIndex đến pageSize để phân trang
            list = list.Skip((pageIndex - 1) * pageSize)
              .Take(pageSize).ToList();
            //Đếm lấy số record thực tế ở đoạn record hiện tại
            int totalRowCurent = model.Count();
            //Các biến để phân trang
            ViewBag.totalRecord = totalRecord;
            ViewBag.pageIndex = pageIndex;
            int recordStartPosition = ((pageIndex - 1) * pageSize) + 1;
            int recordEndPosition = ((recordStartPosition - 1 + pageSize) < totalRecord) ? (recordStartPosition - 1 + pageSize) : recordStartPosition - 1 + totalRowCurent;
            ViewBag.recordStartPosition = recordStartPosition;
            ViewBag.recordEndPosition = recordEndPosition;
            int maxPage = 5;
            int totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize));
            if (totalRecord % pageSize > 0 && totalRecord > pageSize) totalPage += 1;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = pageIndex + 1;
            ViewBag.Prev = pageIndex - 1;
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            return View(list);
        }
        public ActionResult ProductSearch(string keyword, int pageIndex = 1, int pageSize = 6, string sort = "newest")
        {
            if (keyword == null) { keyword = ""; }else keyword=NonUnicode.RemoveUnicode(keyword).ToLower();
            ViewBag.Keyword = keyword;
            //Lấy danh sách danh mục sản phẩm
            ViewBag.ListCategory = new ProductCategoryDao().ListAll().ToList();
            if (sort == "newest") ViewBag.Newest = "selected";
            if (sort == "price-up") ViewBag.PriceUp = "selected";
            if (sort == "price-down") ViewBag.PriceDown = "selected";
            if (sort == "salsest") ViewBag.Salsest = "selected";
            ViewBag.Sort = sort;
            //Lấy danh sách sản phẩm thỏa điều kiện là danh mục sản phẩm
            var model = new ProductDao().ListAll();
            model = model.Where(x => NonUnicode.RemoveUnicode(x.Name).ToLower().Contains(keyword));
            //Lấy tổng các record sản phẩm thỏa điều kiện
            int totalRecord = model.Count();
            //sắp xếp
            switch (sort)
            {
                case "newest": model = model.OrderByDescending(x => x.CreateDate); break;
                case "price-up": model = model.OrderBy(x => (x.PromotionPrice != null && x.PromotionPrice > 0) ? x.PromotionPrice : x.Price); break;
                case "price-down": model = model.OrderByDescending(x => (x.PromotionPrice != null && x.PromotionPrice > 0) ? x.PromotionPrice : x.Price); break;
                case "salest": model = model.OrderByDescending(x => (double)((double)x.PromotionPrice / (double)x.Price)); break;
            }
            //Lấy 1 khoảng trong list sản phẩm từ pageIndex đến pageSize để phân trang
            model = model.Skip((pageIndex - 1) * pageSize)
              .Take(pageSize);
            //Đếm lấy số record thực tế ở đoạn record hiện tại
            int totalRowCurent = model.Count();
            //Các biến để phân trang
            ViewBag.totalRecord = totalRecord;
            ViewBag.pageIndex = pageIndex;
            int recordStartPosition = ((pageIndex - 1) * pageSize) + 1;
            int recordEndPosition = ((recordStartPosition - 1 + pageSize) < totalRecord) ? (recordStartPosition - 1 + pageSize) : recordStartPosition - 1 + totalRowCurent;
            ViewBag.recordStartPosition = recordStartPosition;
            ViewBag.recordEndPosition = recordEndPosition;
            int maxPage = 5;
            int totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize));
            if (totalRecord % pageSize > 0 && totalRecord > pageSize) totalPage += 1;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = pageIndex + 1;
            ViewBag.Prev = pageIndex - 1;
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            return View(model);
        }
        public JsonResult ListName(string q)
        {
            var data = new ProductDao().ListName(q);
            return Json(new
            {
                dataJ = data,
                status = true
            }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult Shop(int pageIndex = 1, int pageSize = 9, string sort = "newest")
        {
            //Lấy danh sách danh mục sản phẩm
            ViewBag.ListCategory = new ProductCategoryDao().ListAll().ToList();
            if (sort == "newest") ViewBag.Newest = "selected";
            if (sort == "price-up") ViewBag.PriceUp = "selected";
            if (sort == "price-down") ViewBag.PriceDown = "selected";
            if (sort == "salsest") ViewBag.Salsest = "selected";
            ViewBag.Sort = sort;
            //Lấy danh sách sản phẩm thỏa điều kiện là danh mục sản phẩm
            var model = new ProductDao().ListAll();
            int totalRecord = model.Count();
            //sắp xếp
            switch (sort)
            {
                case "newest": model = model.OrderByDescending(x => x.CreateDate); break;
                case "price-up": model = model.OrderBy(x => x.PromotionPrice.GetValueOrDefault(0)>0 ? x.PromotionPrice : x.Price); break;
                case "price-down": model = model.OrderByDescending(x => x.PromotionPrice.GetValueOrDefault(0) > 0 ? x.PromotionPrice : x.Price); break;
                case "salsest": model = model.OrderBy(x => x.PromotionPrice.GetValueOrDefault(0)>0?(double)(x.PromotionPrice.Value / x.Price.Value):1); break;
            }
            //Lấy 1 khoảng trong list sản phẩm từ pageIndex đến pageSize để phân trang
            model = model.Skip((pageIndex - 1) * pageSize)
              .Take(pageSize);
            //Đếm lấy số record thực tế ở đoạn record hiện tại
            int totalRowCurent = model.Count();
            //Các biến để phân trang
            ViewBag.totalRecord = totalRecord;
            ViewBag.pageIndex = pageIndex;
            int recordStartPosition = ((pageIndex - 1) * pageSize) + 1;
            int recordEndPosition = ((recordStartPosition - 1 + pageSize) < totalRecord) ? (recordStartPosition - 1 + pageSize) : recordStartPosition - 1 + totalRowCurent;
            ViewBag.recordStartPosition = recordStartPosition;
            ViewBag.recordEndPosition = recordEndPosition;
            int maxPage = 5;
            int totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize));
            if (totalRecord % pageSize > 0 && totalRecord > pageSize) totalPage += 1;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = pageIndex + 1;
            ViewBag.Prev = pageIndex - 1;
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            return View(model);
        }
        public ActionResult PrintView(long id)
        {
            var product = new ProductDao().GetByIDView(id);
            return View(product);
        }
    }
}