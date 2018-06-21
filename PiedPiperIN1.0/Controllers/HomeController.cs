using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PiedPiperIN1._0.Models;

namespace PiedPiperIN1._0.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        //Hi this is my first commit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(user_profile objUser)
        {
            if (ModelState.IsValid)
            {
                using (PiedPiperINEntities db = new PiedPiperINEntities())
                {
                    var obj = db.user_profile.Where(a => a.Email.Equals(objUser.Email) && a.Password.Equals(objUser.Password)).FirstOrDefault();
                    if (obj != null)
                    {
                        Session["Id"] = obj.ID.ToString();
                        Session["Email"] = obj.Email.ToString();
                        Session["Role"] = obj.Role.ToString();
                        return RedirectToAction("UserDashBoard");
                    }
                }
            }
            return View(objUser);
        }


        public ActionResult UserDashBoard()
        {
            if (Session["Email"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    }
}