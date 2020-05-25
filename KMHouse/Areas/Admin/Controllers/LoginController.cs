using Common;
using Models.DAO;
using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace KMHouse.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var res = dao.Login(model.UserName, Encryptor.MD5Hash(Encryptor.EncodeTo64(model.Password)), true);
                if (res == 1)
                {
                    var user = dao.GetUserExist(model.UserName);
                    var userSession = new UserLoginSession();
                    userSession.UserName = user.UserName;
                    userSession.UserID = user.ID;
                    userSession.UserGroupID = user.UserGroupID;
                    userSession.Name = user.Name;
                    var listCredentials = dao.GetListCredential(model.UserName);

                    Session.Add(ConstantSession.SESSION_CREDENTIALS, listCredentials);
                    Session.Add(ConstantSession.USER_SESSION, userSession);
                    ViewBag.UserName = ((UserLoginSession)Session[ConstantSession.USER_SESSION]).UserName;
                    return RedirectToAction("Index", "Default");
                }
                else if (res == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại!");
                }
                else if (res == -1)
                {
                    ModelState.AddModelError("", "Tài khoản đang tạm khóa!");
                }
                else if (res == -2)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng!");
                }
                else if (res == -3)
                {
                    ModelState.AddModelError("", "Tài khoản bạn không có quyền truy cập trang này!");
                }

            }
            else
            {
                ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin");
            }
            return View("Index");
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session[ConstantSession.USER_SESSION] = null;
            return RedirectToAction("Index", "Login");
        }
    }
}