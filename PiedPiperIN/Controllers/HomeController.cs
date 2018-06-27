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
        //This is Fahad committing directly into the master branch
        //Kitkat commit
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
            PiedPiperINEntities db = new PiedPiperINEntities();

            return View(db.product.ToList());
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
        [HttpPost]
        public ActionResult addtocart(string qty)
        {
            // PiedPiperINEntities db = new PiedPiperINEntities();
           var id= Session["pid"];
            var  name= Session["product_name"];
            using (PiedPiperINEntities db = new PiedPiperINEntities())
            {
                cart_view cart = new cart_view();
                SqlConnection conn = new SqlConnection("Data Source=SHAREPOINT;Initial Catalog=PiedPiperIN;Persist Security Info=True;User ID=sa;Password=B@cstech135");
                conn.Open();
                SqlCommand sqlCmd = new SqlCommand("addtocart", conn);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue(qty,cart.Quantity);
                // int result = command.ExecuteNonQuery();


                ////////////////////////////////////////////////////
                cart.prdouct_id = Convert.ToInt32(id);
                cart.Quantity = Convert.ToInt32(qty);
                
            }

                return View();
        }
}
}