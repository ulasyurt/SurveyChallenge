using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace surveychallenge.Controllers
{
    public class SecurityController : Controller
    {
        SurveyChallengeEntities db = new SurveyChallengeEntities();

        [Route("~/security")]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(User user)
        {
            if(db.User.FirstOrDefault(x=>x.UserName==user.UserName && x.UserPassword == user.UserPassword) != null)
            {
                FormsAuthentication.SetAuthCookie(user.UserName, false);
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { failure = true }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}