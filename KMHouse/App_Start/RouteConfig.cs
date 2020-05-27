using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace KMHouse
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
               name: "Print View",
               url: "print-view/{id}",
               defaults: new { controller = "ProductHome", action = "PrintView", id = UrlParameter.Optional },
               namespaces: new[] { "KMHouse.Controllers" }
           );
            routes.MapRoute(
               name: "Contact",
               url: "lien-he/",
               defaults: new { controller = "Info", action = "Contact", id = UrlParameter.Optional },
               namespaces: new[] { "KMHouse.Controllers" }
           );
            routes.MapRoute(
               name: "About",
               url: "gioi-thieu/",
               defaults: new { controller = "Info", action = "About", id = UrlParameter.Optional },
               namespaces: new[] { "KMHouse.Controllers" }
           );
            routes.MapRoute(
               name: "News Of Category",
               url: "tin-tuc/{MeTaTitle}-{cateId}",
               defaults: new { controller = "NewsClient", action = "NewsOfCategory", id = UrlParameter.Optional },
               namespaces: new[] { "KMHouse.Controllers" }
           );
            routes.MapRoute(
               name: "News Tag",
               url: "tag/{tagId}",
               defaults: new { controller = "NewsClient", action = "NewsTag", id = UrlParameter.Optional },
               namespaces: new[] { "KMHouse.Controllers" }
           );
            routes.MapRoute(
               name: "Prodct Tag",
               url: "ptag/{tagId}",
               defaults: new { controller = "ProductHome", action = "ProductTag", id = UrlParameter.Optional },
               namespaces: new[] { "KMHouse.Controllers" }
           );
            routes.MapRoute(
               name: "News Detail",
               url: "chi-tiet-tin/{Metatitle}-{id}",
               defaults: new { controller = "NewsClient", action = "NewsDetail", id = UrlParameter.Optional },
               namespaces: new[] { "KMHouse.Controllers" }
           );
            routes.MapRoute(
               name: "Payment Seccess",
               url: "hoan-thanh/",
               defaults: new { controller = "Cart", action = "Success", id = UrlParameter.Optional },
               namespaces: new[] { "KMHouse.Controllers" }
           );
            routes.MapRoute(
               name: "Payment",
               url: "thanh-toan/",
               defaults: new { controller = "Cart", action = "Payment", id = UrlParameter.Optional },
               namespaces: new[] { "KMHouse.Controllers" }
           );
            routes.MapRoute(
               name: "Change Password",
               url: "doi-mat-khau/",
               defaults: new { controller = "UserClient", action = "ChangePassword", id = UrlParameter.Optional },
               namespaces: new[] { "KMHouse.Controllers" }
           );
            routes.MapRoute(
               name: "Forget Password",
               url: "quen-mat-khau/",
               defaults: new { controller = "UserClient", action = "ForgetPassword", id = UrlParameter.Optional },
               namespaces: new[] { "KMHouse.Controllers" }
           );
            routes.MapRoute(
               name: "Account",
               url: "tai-khoan/",
               defaults: new { controller = "Home", action = "Account", id = UrlParameter.Optional },
               namespaces: new[] { "KMHouse.Controllers" }
           );
            routes.MapRoute(
                name: "Show Cart",
                url: "gio-hang/",
                defaults: new { controller = "Cart", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "KMHouse.Controllers" }
            );
            routes.MapRoute(
                name: "Add Cart",
                url: "them-gio-hang/",
                defaults: new { controller = "Cart", action = "AddItem", id = UrlParameter.Optional },
                namespaces: new[] { "KMHouse.Controllers" }
            );
            routes.MapRoute(
                name: "Register",
                url: "dang-ky/",
                defaults: new { controller = "UserClient", action = "Register", id = UrlParameter.Optional },
                namespaces: new[] { "KMHouse.Controllers" }
            );
            routes.MapRoute(
                name: "Login",
                url: "dang-nhap/",
                defaults: new { controller = "UserClient", action = "Login", id = UrlParameter.Optional },
                namespaces: new[] { "KMHouse.Controllers" }
            );
            routes.MapRoute(
                name: "News",
                url: "tin-tuc/",
                defaults: new { controller = "NewsClient", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "KMHouse.Controllers" }
            );
            routes.MapRoute(
                name: "Search",
                url: "tim-kiem/",
                defaults: new { controller = "ProductHome", action = "ProductSearch", id = UrlParameter.Optional },
                namespaces: new[] { "KMHouse.Controllers" }
            );
            routes.MapRoute(
                name: "Product Of Category",
                url: "san-pham/{MetaTitle}-{cateId}",
                defaults: new { controller = "ProductHome", action = "ProductOfCategory", id = UrlParameter.Optional },
                namespaces: new[] { "KMHouse.Controllers" }
            );
            routes.MapRoute(
                name: "Shop",
                url: "shop/",
                defaults: new { controller = "ProductHome", action = "Shop", id = UrlParameter.Optional },
                namespaces: new[] { "KMHouse.Controllers" }
            );
            routes.MapRoute(
                name: "product Detail",
                url: "chi-tiet/{MetaTitle}-{id}",
                defaults: new { controller = "ProductHome", action = "ProductDetail", id = UrlParameter.Optional },
                namespaces: new[] { "KMHouse.Controllers" }
            );
            
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",//{controller}/{action}/{id}
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Default2",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
