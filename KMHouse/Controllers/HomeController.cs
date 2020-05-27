using Models.DAO;
using Models.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;
using Common;
using System.Xml.Linq;

namespace KMHouse.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Slide = new SliderDao().ListAll();
            ViewBag.Product = new ProductDao().ListAll();
            ViewBag.ProductCategory = new ProductCategoryDao().ListAll();
            return View();
        }
        [ChildActionOnly]
        public PartialViewResult MiniCart()//tính toán giỏ hàng hiện trên trang chủ
        {
            var cart = Session[ConstantSession.CartSession];
            var list = new List<CartItemViewModel>();
            if (cart != null)
            {
                list = (List<CartItemViewModel>)cart;

            }
            return PartialView(list);
        }
        public ActionResult Account()
        {
            var userCurrent = (UserLoginSession)Session[ConstantSession.USER_CLIENT_SESSION];
            var user = new UserDao().GetDetailById(userCurrent.UserID);

            return View(user);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
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
        //Hàm lấy giá trị hiển thị QuickView
        public JsonResult QuickView(long id)
        {
            var model = new ProductDao().GetByIDView(id);
            ViewBag.ListImage = this.LoadImages(id);
            ViewBag.QuickViewProduct = model;
            return Json(new
            {
                data = model,
                status = true,
            }, JsonRequestBehavior.AllowGet);
        }
    }
}