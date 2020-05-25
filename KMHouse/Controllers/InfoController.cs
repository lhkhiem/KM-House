using Common;
using Models.DAO;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KMHouse.Controllers
{
    public class InfoController : Controller
    {
        // GET: About
        public ActionResult About()
        {
            var model = new AboutDao().GetAbout();
            return View(model);
        }
        public ActionResult Contact()
        {
            var model = new ContactDao().ListAll().FirstOrDefault(x => x.Status = true);
            return View(model);
        }
        public JsonResult SendFeedback(string name, string address, string mobile, string email, string title, string message)
        {
            try
            {
                var feedback = new Feedback();
                feedback.Name = name;
                feedback.Address = address;
                feedback.Phone = mobile;
                feedback.CreateDate = DateTime.Now;
                feedback.Email = email;
                feedback.Content = message;
                feedback.Status = false;

                var id = new FeedbackDao().Insert(feedback);

                string content = System.IO.File.ReadAllText(Server.MapPath("~/Asset/Client/template/feedback.html"));
                content = content.Replace("{{Title}}", title);
                content = content.Replace("{{CustomerName}}", name);
                content = content.Replace("{{Email}}", email);
                content = content.Replace("{{Feedback}}", message);

                var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
                new MailHelper().SendMailFeedback(toEmail, "Phản hồi", content);
                return Json(new
                {
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    status = false
                }, JsonRequestBehavior.AllowGet);
            }

            
        }

    }
}