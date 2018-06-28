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
        [HttpGet]
        public ActionResult Invoice()
        {
            PiedPiperINEntities db = new PiedPiperINEntities();
            DashboardViewModel dashboardView = new DashboardViewModel();
            int uid = Convert.ToInt32(Session["id"]);
            dashboardView.Cart = db.cart_view.Where(m => m.id == uid).ToList();
            float total_taxable = 0;

            order order = new order();
            order.User_ID = uid;
            float total_price = 0;
            Random rnd = new Random();
            int orderno = rnd.Next(1000, 100000);
            foreach (var x in dashboardView.Cart.Where(m=>m.id == uid))
            {
                x.taxable_price = (float)(x.price) * (100+ x.category)/100;
                total_taxable += (float) x.taxable_price;
                order.Product_List += x.product_name+"("+x.Quantity+"), ";
                total_price += (float)x.price;
            }
            order.order_number = orderno;
            order.taxableprice = total_taxable;
            order.totalprice = total_price;
            db.orders.Add(order);
            db.SaveChanges();
            string name;
            string Address;
            string email;
            foreach(var x in db.user_profile.Where(m => m.ID == uid))
            {
                name = x.Name;
                Address = x.Address;
                Session["name"] = name;
                Session["Address"] = Address;

            }
            
            Session["order_no"] = orderno;
            Session["taxable"] = total_taxable;
            return View(dashboardView);
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
                Session["id"] = obj.ID.ToString();

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
        [HttpGet]
        public ActionResult UserDashBoard()
        {
            PiedPiperINEntities db = new PiedPiperINEntities();

            DashboardViewModel dashboardView = new DashboardViewModel();
            dashboardView.Product = db.products.ToList();
            int uid = Convert.ToInt32(Session["id"]);
            dashboardView.Cart = db.cart_view.Where(k => k.id == uid).ToList();
            int total = 0;
            int total_products = 0;
            foreach (var x in db.cart_view.Where(k => k.id == uid))
            {
                total = total + (int)x.price;
                total_products = total_products + (int)x.Quantity;
            }
            Session["total"] = total;
            Session["qty"] = total_products;
            total = 0;
            total_products = 0;
            return View(dashboardView);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create_user([Bind(Include = "Name,Email,Password,Address")] user_profile user_profile)
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
        public ActionResult addtocart(string pid, string pname, string qty, string price, string category)
        {
            // PiedPiperINEntities db = new PiedPiperINEntities();


            using (PiedPiperINEntities db = new PiedPiperINEntities())
            {
                cart_view cart = new cart_view();
                int pro_id = int.Parse(pid);
                cart.prdouct_id = int.Parse(pid);
                int usid = Convert.ToInt32(Session["id"]);
                cart.id = Convert.ToInt32(Session["id"]);
                cart.product_name = pname;
                cart.Quantity = int.Parse(qty);
                cart.price = int.Parse(price) * int.Parse(qty);
                cart.category = Convert.ToInt32(category);

              
                var obj = db.cart_view.Where(m => m.prdouct_id == pro_id && m.id == usid).FirstOrDefault();
                if (obj == null)
                {
                    db.cart_view.Add(cart);
                    db.SaveChanges();
                }
                else
                {
                    int qty_b = 0;
                    var count = db.cart_view.Where(m => m.prdouct_id == pro_id && m.id == usid).ToList();
                    foreach (var x in count)
                    {
                        qty_b = (int)x.Quantity;

                    }

                    cart = db.cart_view.FirstOrDefault(m => m.prdouct_id == pro_id && m.id == usid);

                    cart.Quantity = qty_b + int.Parse(qty);
                    cart.price = (qty_b + int.Parse(qty)) * int.Parse(price);
                    db.SaveChanges();

                }


            }
            PiedPiperINEntities db1 = new PiedPiperINEntities();
            DashboardViewModel dashboardView = new DashboardViewModel();
            int uid = Convert.ToInt32(Session["id"]);
            dashboardView.Cart = db1.cart_view.Where(k => k.id == uid).ToList();
            dashboardView.Product = db1.products.ToList();
            int total = 0;
            int total_products = 0;
            foreach (var x in db1.cart_view.Where(k => k.id == uid))
            {
                total = total + (int)x.price;
                total_products = total_products + (int)x.Quantity;
            }
            Session["total"] = total;
            Session["qty"] = total_products;
            total = 0;
             total_products = 0;
            return View("UserDashBoard", dashboardView);

        }
        //[HttpGet]
        //public ActionResult updateCart()
        //{
        //    PiedPiperINEntities db = new PiedPiperINEntities();
        //    DashboardViewModel dashboardView = new DashboardViewModel();
        //    int uid = Convert.ToInt32(Session["id"]);
        
        //    dashboardView.Cart = db.cart_view.Where(k => k.id == uid).ToList();
        //    dashboardView.Product = db.product.ToList();
        //    int total = 0;
        //    foreach (var x in db.cart_view.Where(k => k.id == uid))
        //    {
        //        total = total + (int)x.price;
        //    }
        //    Session["total"] = total;
        //    total = 0;
        //    return View("UserDashBoard", dashboardView);
        //}

        [HttpPost]
        
            public ActionResult updateCart(string pid, string pname, string qty, string price)
        {
            PiedPiperINEntities db = new PiedPiperINEntities();
            DashboardViewModel dashboardView = new DashboardViewModel();
            int prid = Convert.ToInt32(pid);
            cart_view f = db.cart_view.FirstOrDefault(x => x.prdouct_id == prid);
            db.cart_view.Remove(f);
            db.SaveChanges();
            int uid = Convert.ToInt32(Session["id"]);
            dashboardView.Cart = db.cart_view.Where(k => k.id == uid).ToList();
            dashboardView.Product = db.products.ToList();
            int total = 0;
            int total_products = 0;
            foreach (var x in db.cart_view.Where(k => k.id == uid))
            {
                total = total + (int)x.price;
                total_products = total_products + (int)x.Quantity;
            }
            Session["total"] = total;
            Session["qty"] = total_products;
            total = 0;
            total_products = 0;
            return View("UserDashBoard", dashboardView);

        }
    }
}