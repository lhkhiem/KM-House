using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Common;
using Models.DAO;
using Models.EF;
using Models.ViewModels;

namespace KMHouse.Controllers
{
    public class CartController : Controller
    {
        private const string CartSession = "CartSession";
        public ActionResult Index()
        {
            var cart = Session[CartSession];
            var list = new List<CartItemViewModel>();
            if (cart != null)
            {
                list = (List<CartItemViewModel>)cart;
            }
            return View(list);
        }
        public JsonResult DeleteAll()
        {
            Session[CartSession] = null;
            return Json(new
            {
                status = true
            });
        }
        public JsonResult Delete(long id)
        {
            var sessionCart = (List<CartItemViewModel>)Session[CartSession];
            sessionCart.RemoveAll(x => x.Product.ID == id);
            Session[CartSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }
        public JsonResult Update(string cartModel)
        {
            var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItemViewModel>>(cartModel);
            var sessionCart = (List<CartItemViewModel>)Session[CartSession];
            foreach (var item in sessionCart)
            {
                var jsonItem = jsonCart.SingleOrDefault(x => x.Product.ID == item.Product.ID);
                if (jsonItem != null)
                {
                    item.Quantity = jsonItem.Quantity;

                }
            }
            return Json(new
            {
                status = true
            });
        }
        [HttpGet]
        public ActionResult Payment()
        {
            var cart = Session[CartSession];
            var list = new List<CartItemViewModel>();
            if (cart != null)
            {
                list = (List<CartItemViewModel>)cart;
            }
            return View(list);
        }
        [HttpPost]
        public ActionResult Payment(string shipName, string mobile, string address)
        {
            decimal total = 0;
            var user = ((UserLoginSession)Session[ConstantSession.USER_CLIENT_SESSION]);
            var order = new Order();
            order.CreateDate = DateTime.Now;
            order.UserID = user.UserID;
            order.ShipAddress = address;
            order.ShipName = shipName;
            order.ShipMobile = mobile;
            order.ShipEmail = user.Email;
            order.Status = 1;
            try
            {
                var id = new OrderDao().Insert(order);
                var cart = (List<CartItemViewModel>)Session[CartSession];
                var detailDao = new OrderDetailDao();
                foreach (var item in cart)
                {
                    var orderDetail = new OrderDetail();

                    orderDetail.ProductID = item.Product.ID;
                    orderDetail.OrderID = id;
                    var price = item.Product.PromotionPrice.GetValueOrDefault(0) > 0 ? item.Product.PromotionPrice : item.Product.Price;
                    orderDetail.Price = price.Value;
                    orderDetail.Quantity = item.Quantity;
                    detailDao.Insert(orderDetail);

                    total += (price.Value * item.Quantity);
                }
                string content = System.IO.File.ReadAllText(Server.MapPath("~/Asset/Client/template/neworder.html"));
                content = content.Replace("{{CustomerName}}", shipName);
                content = content.Replace("{{Phone}}", mobile);
                content = content.Replace("{{Email}}", user.Email);
                content = content.Replace("{{Address}}", address);
                content = content.Replace("{{Total}}", total.ToString("N0"));

                var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
                new MailHelper().SendMail(user.Email, "Đơn hàng mới", content);//gui mail khach hang
                //new MailHelper().SendMail(toEmail, "Đơn hàng mới", content);//gui mail cho quan tri vien
            }
            catch (Exception)
            {
                //ghi log
                //return Redirect("/trang-loi");
                throw;
            }
            Session[CartSession] = null;
            return Redirect("/hoan-thanh");
        }
        
        public ActionResult Success()
        {
            return View();
        }
        public JsonResult AddItem(long productId, int quantity)
        {
            if(Session[ConstantSession.USER_CLIENT_SESSION] != null)
            {
                var product = new ProductDao().GetByID(productId);
                if (product.Price > 0)
                {
                    var cart = Session[CartSession];
                    if (cart != null)
                    {
                        var list = (List<CartItemViewModel>)cart;
                        if (list.Exists(x => x.Product.ID == productId))
                        {
                            foreach (var item in list)
                            {
                                if (item.Product.ID == productId)
                                {
                                    item.Quantity += quantity;
                                }
                            }
                        }
                        else
                        {
                            //tao moi doi tuong CartItem
                            var item = new CartItemViewModel();
                            item.Product = product;
                            item.Quantity = quantity;
                            list.Add(item);
                        }
                        //Gan vao Session
                        Session[CartSession] = list;
                        return Json(new
                        {
                            miniCart = list,
                            status = 1
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //tao moi doi tuong CartItem
                        var item = new CartItemViewModel();
                        item.Product = product;
                        item.Quantity = quantity;
                        var list = new List<CartItemViewModel>();
                        list.Add(item);
                        //Gan vao Session
                        Session[CartSession] = list;
                        return Json(new
                        {
                            miniCart = list,
                            status = 1
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new
                    {
                        status = -1
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new
                {
                    status = 0
                }, JsonRequestBehavior.AllowGet);
            }
            
        }
    }
}