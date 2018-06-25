using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using PiedPiperIN.Models;
namespace PiedPiperIN.Controllers
{
    public class HomeController : Controller
    {
        public object Email { get; private set; }

        // GET: Home
        //first Commit is here
        //this is my second commit
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        //Firstcommit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(user_profile objUser)
        {
            Session["htmlStr"] = "<table>";

            PiedPiperINEntities db = new PiedPiperINEntities();

            var obj = db.user_profile.Where(a => a.Email.Equals(objUser.Email) && a.Password.Equals(objUser.Password)).FirstOrDefault();

            if (obj != null)
            {
                Session["Id"] = obj.ID.ToString();
                Session["Email"] = obj.Email.ToString();
                Session["Role"] = obj.Role.ToString();
                Session["Name"] = obj.Name.ToString();
                var listdata = db.products;
                foreach (var x in listdata)
                {
                    Session["htmlStr"] += "<form><tr style='font-size:20px;'> <td style='width:100px;border: 1px solid black;'><img src='/content/" + x.Product_Pic + "' width='100' height='100'></td> <td style='width:100px;border: 1px solid black;'>" + x.Product_ID + "</td><td style='width:100px;border: 1px solid black;' >" + x.Product_Name + "</td> <td style='width:100px;border: 1px solid black;'><input type='submit' value='Add'/></td><td style='width:40px;border: 1px solid black;'><input style='width:40px;' type='text'/></td></tr> </form>";

                }
                Session["htmlStr"] += "</table>";




                // (repeat the same for all)



                return RedirectToAction("UserDashBoard");

            }

            else
            {
                Session["wrong"] = "true";
                return RedirectToAction("Index");
            }


            


                return View(objUser);


            
        }
        /// <summary>
        /// /
        /// </summary>
        /// <returns></returns>
        ///
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
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create([Bind(Include = "Name,Email,Password,Address")] user_profile user_profile)
    {
        PiedPiperINEntities db = new PiedPiperINEntities();
        if (ModelState.IsValid)
        {
            db.user_profile.Add(user_profile);
            db.SaveChanges();
            Session["state"] = "true";
            return RedirectToAction("Index");
        }

        return View(user_profile);
    }
    public ActionResult Logout()
    {
        Session["Email"] = null;
        return RedirectToAction("Index");

    }
}
}