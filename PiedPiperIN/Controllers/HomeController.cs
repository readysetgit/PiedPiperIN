using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using PiedPiperIN.Models;
namespace PiedPiperIN.Controllers
{
    //
    [Authorize]
    public class HomeController : Controller
    {
        public object Email { get; private set; }

        // GET: Home
        //first Commit is here
        //this is my second commit
        //This is Fahad committing directly into the master branch
        //Kitkat commit

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Create()
        {
            return View();
        }
        [HttpGet]
        [Authorize]
        public ActionResult Invoice()
        {
            PiedPiperINEntities db = new PiedPiperINEntities();
            DashboardViewModel dashboardView = new DashboardViewModel();
            if (Session["state"].ToString() == "true")
            {

                int uid = Convert.ToInt32(Session["id"]);

                dashboardView.Cart = db.cart_view.Where(m => m.id == uid).ToList();
                double total_taxable = 0;

                order order = new order();
                order.User_ID = uid;
                double total_price = 0;
                double total_price_value = 0;
                int isCouponApplied = 0;
                Random rnd = new Random();
                int orderno = rnd.Next(1000, 100000);
                foreach (var x in db.cart_view.Where(m => m.id == uid))
                {
                    x.taxable_price = Math.Round((float)((x.price) * ((100 + (float)x.category) / 100)),2);
                    total_taxable += (float)x.taxable_price;
                    order.Product_List += x.product_name + "(" + x.Quantity + "), ";
                    total_price += (float)x.discounted_price;
                    total_price_value += (float)x.price;
                    if(x.coupon_applied==1)
                    {
                        Session["isCouponApplied"] = "true";
                        ++isCouponApplied;
                    }
                }
                if(isCouponApplied==0)
                {
                    Session["isCouponApplied"] = "false";
                }
                order.order_number = orderno;
                order.taxableprice = Math.Round(total_taxable,4);
                order.totalprice = Math.Round(total_price, 2);
                db.orders.Add(order);
                db.SaveChanges();
                

                string name;
                string Address;
           
                foreach (var x in db.user_profile.Where(m => m.ID == uid))
                {
                    name = x.Name;
                    Address = x.Address;
                    Session["name"] = name;
                    Session["Address"] = Address;

                }
                total_taxable = Math.Round(total_taxable, 2);
                total_price = Math.Round(total_price,2);
                total_price_value = Math.Round(total_price_value, 2);
              
                Session["total"] = total_price_value;
                Session["order_no"] = orderno;
                Session["taxable"] = total_taxable;
                Session["total_price_value"] = total_price;

                Session["total_payable"] = total_taxable - total_price_value + total_price;

                Session["state"] = false;
                dashboardView.Cart = db.cart_view.Where(m => m.id == uid).ToList();
                foreach (var x in db.cart_view.Where(m => m.id == uid))
                {
                    db.cart_view.Remove(x);

                }
                db.SaveChanges();
               
                return View("Invoice",dashboardView);
            }
            else
            {
                int uid = Convert.ToInt32(Session["id"]);
                dashboardView.Cart = db.cart_view.Where(m => m.id == uid).ToList();
                return View(dashboardView);
            }

        }
        //Firstcommit
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Login(user_profile objUser)
        {

            using (PiedPiperINEntities db = new PiedPiperINEntities())
            {
                var user = db.user_profile.Where(a => a.Email.Equals(objUser.Email) && a.Password.Equals(objUser.Password)).FirstOrDefault();
                if (user != null)
                {
                    Session["name"] = user.Name;
                    Session["address"] = user.Address;
                    FormsAuthentication.SetAuthCookie(user.Email, objUser.RememberMe);
                    if (user.Role=="admin")
                        {
                        return RedirectToAction("UploadProduct", "Product");
                    }
                    else
                    {
                        return RedirectToAction("UserDashBoard", "Home");
                    }
                }
            }
            ModelState.Remove("Password");
            return View();




        }
      
        [HttpGet]
        [Authorize]
        public ActionResult UserDashBoard()
        {
            Session["state"] = "true";
            PiedPiperINEntities db = new PiedPiperINEntities();

            DashboardViewModel dashboardView = new DashboardViewModel();
            dashboardView.Product = db.products.ToList();
            int uid = Convert.ToInt32(Session["id"]);
            dashboardView.Cart = db.cart_view.Where(k => k.id == uid).ToList();
            double total = 0;
            int total_products = 0;
            foreach (var x in db.cart_view.Where(k => k.id == uid))
            {
                total = total + Math.Round( (double)x.price,2);
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
            user_profile.Role = "user";
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
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");

        }

        [HttpPost]
        [Authorize]
        public ActionResult addtocart(string pid, string pname, string qty, string price, string category)
        {
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
                cart.discounted_price= int.Parse(price) * int.Parse(qty);
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
                        x.coupon_applied = 0;
                       

                    }

                    cart = db.cart_view.FirstOrDefault(m => m.prdouct_id == pro_id && m.id == usid);

                    cart.Quantity = qty_b + int.Parse(qty);
                    cart.price = (qty_b + int.Parse(qty)) * int.Parse(price);
                    foreach (var x in count)
                    {

                        x.discounted_price = x.price;


                    }
                    db.SaveChanges();

                }


            }

            PiedPiperINEntities db1 = new PiedPiperINEntities();
            DashboardViewModel dashboardView = new DashboardViewModel();
            int uid = Convert.ToInt32(Session["id"]);
            dashboardView.Cart = db1.cart_view.Where(k => k.id == uid).ToList();
            dashboardView.Product = db1.products.ToList();
            float total = 0;
            int total_products = 0;
            foreach (var x in db1.cart_view.Where(k => k.id == uid))
            {
                total = total + (float)x.price;
                total_products = total_products + (int)x.Quantity;
            }
            Session["total"] = total;
            Session["qty"] = total_products;
            total = 0;
            total_products = 0;
            return View("UserDashBoard", dashboardView);

        }
       

        [HttpPost]
        [Authorize]
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
            double total = 0;
            double total_products = 0;
            foreach (var x in db.cart_view.Where(k => k.id == uid))
            {
               total =  total +  Math.Round((double)x.price,2);
             
                total_products = total_products + (int)x.Quantity;
            }
            Session["total"] = total;
            Session["qty"] = total_products;
            total = 0;
            total_products = 0;
            return View("UserDashBoard", dashboardView);

        }

        public ActionResult ApplyCoupon(string discount_value)
        {
            DashboardViewModel dashboardView = new DashboardViewModel();

            PiedPiperINEntities db = new PiedPiperINEntities();

            try
            {
                cart_view cart = new cart_view();
                int usid = Convert.ToInt32(Session["id"]);

                double dis = (100 - float.Parse(discount_value)) / 100;
                float total = 0;
                int flag = 0;
                foreach (var x in db.cart_view.Where(a => a.id == usid).ToList())
                {
                    if (x.coupon_applied == 0)
                    {
                        x.discounted_price = Math.Round((double)x.price * dis, 2);
                        x.coupon_applied = 1;
                        Session["coupon_Applied"] = "true";
                        ++flag;
                    }
                    

                    total += (float)x.discounted_price;
                }
                db.SaveChanges();
                Session["total"] = total;
                if (flag == 0)
                {
                    Session["coupon_Applied"] = "false";
                    Session["discount_value"] = "0";
                }
                else
                {
                    Session["discount_value"] = float.Parse(discount_value);
                }
                dashboardView.Cart = db.cart_view.Where(k => k.id == usid).ToList();
                dashboardView.Product = db.products.ToList();
               

            }
            catch
            {
                Session["coupon_Applied"] = "false";
            }

            return View("UserDashBoard",dashboardView);


        }

 } } 
