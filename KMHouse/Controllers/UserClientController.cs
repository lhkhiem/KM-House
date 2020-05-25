using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models.DAO;
using Models.ViewModels;
using Common;
using Models.EF;

namespace KMHouse.Controllers
{
    public class UserClientController : Controller
    {
        private Uri RedirectUri
        {
            get
            {
                var uriBiulder = new UriBuilder(Request.Url);
                uriBiulder.Query = null;
                uriBiulder.Fragment = null;
                uriBiulder.Path = Url.Action("FacebookCallback");
                return uriBiulder.Uri;
            }
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModelClient model)
        {
            if (ModelState.IsValid)
            {

                var dao = new UserDao();
                var res = dao.LoginByEmail(model.Email, Encryptor.MD5Hash(Encryptor.EncodeTo64(model.Password)));
                if (res == 1)
                {
                    var user = dao.GetDetailByEmail(model.Email);
                    var userSession = new UserLoginSession();
                    userSession.UserName = user.UserName;
                    userSession.Email = user.Email;
                    userSession.UserID = user.ID;
                    userSession.UserGroupID = user.UserGroupID;
                    userSession.Name = user.Name;
                    Session.Add(ConstantSession.USER_CLIENT_SESSION, userSession);
                    return Redirect("/");
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

            }
            else
            {
                ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin");
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                if (dao.CheckUserExist(model.UserName))
                {
                    ModelState.AddModelError("", "Tên đăng nhập tồn tại");
                }
                else if (dao.CheckEmailExist(model.Email))
                {
                    ModelState.AddModelError("", "Email đã tồn tại");
                }
                else
                {
                    var user = new UserViewModel();
                    user.UserName = model.UserName;
                    user.Name = model.Name;
                    user.Password = Encryptor.MD5Hash(Encryptor.EncodeTo64(model.Password));
                    user.Phone = model.Phone;
                    user.Email = model.Email;
                    user.Address = model.Address;
                    user.CreateDate = DateTime.Now;
                    user.UserGroupID = "GUEST";
                    user.CreateBy = "Myself";
                    user.Status = true;
                    var result = dao.Insert(user);
                    if (result > 0)
                    {
                        ViewBag.Success = "Đăng ký thành công";
                        model = new RegisterModel();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Đăng ký không thành công");
                    }
                }
            }
            return View(model);
        }
        public ActionResult Logout()
        {
            Session[ConstantSession.USER_CLIENT_SESSION] = null;
            return Redirect("/");
        }
        [HttpGet]
        public ActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgetPassword(string email)
        {
            var model = new UserDao().CheckEmailExist(email);
            if (model)
            {
                string password = RadomString.GetUniqueKey(6);
                var user = new UserDao().GetDetailByEmail(email);
                string content = System.IO.File.ReadAllText(Server.MapPath("~/Asset/Client/template/newpassword.html"));
                content = content.Replace("{{CustomerName}}", user.Name.ToString());
                content = content.Replace("{{Email}}", email);
                content = content.Replace("{{Password}}", password);

                new MailHelper().SendMailRecoveryPassword(email, "KStore - Khôi phục mật khẩu", content);//gui mail khach hang
                //cập nhật mật khẩu
                var res = new UserDao().UpdatePassword(user.ID, Encryptor.MD5Hash(Encryptor.EncodeTo64(password)));
                if (res)
                {
                    ViewBag.Info = "<span class='text-success'>Gửi thành công! Vui lòng kiểm tra email của bạn.</span>";
                }
            }
            else
            {
                ViewBag.Info = "<span class='text-danger'>Email không tồn tại!</span>";
            }
            return View();
        }
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(ChangePassViewModel model)
        {
            if (ModelState.IsValid)
            {
                var ckemail = new UserDao().CheckEmailExist(model.Email);
                if (ckemail)
                {
                    var user = new UserDao().GetDetailByEmail(model.Email);
                    if (user.Password.Equals(Encryptor.MD5Hash(Encryptor.EncodeTo64(model.Password))))
                    {
                        var res = new UserDao().UpdatePassword(user.ID, Encryptor.MD5Hash(Encryptor.EncodeTo64(model.NewPassword)));
                        if (res)
                        {
                            ViewBag.Info = "<span class='text-success'>Đổi mật khẩu thành công</span>";
                        }
                    }
                    else
                    {
                        ViewBag.Info = "<span class='text-danger'>Mật khẩu cũ không đúng.</span>";
                    }
                }
                else
                {
                    ViewBag.Info = "<span class='text-success'>Email không đúng.</span>";
                }
            }
            return View();
        }
    }
}