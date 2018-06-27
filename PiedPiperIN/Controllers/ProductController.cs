using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PiedPiperIN.Models;


namespace PiedPiperIN.Controllers
{
    public class ProductController : Controller
    {

        [HttpGet]
        public ActionResult UploadProduct()
        {
            PiedPiperINEntities productdb = new PiedPiperINEntities();
            DashboardViewModel dash = new DashboardViewModel();
            
            dash.Product = productdb.product.ToList();
            dash.Cart = productdb.cart_view.ToList();

            ViewBag.Head = "Upload Product";
                return View(dash);
            
        }

        
        [HttpPost]
        public ActionResult UploadProduct(DashboardViewModel newproduct, HttpPostedFileBase file)
        {
            ViewBag.Head = "Upload Product";
            
            {
                DashboardViewModel productmodel = new DashboardViewModel();

                if (file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("/Content/"), _FileName);
                    productmodel.Product. = _FileName;
                    //productmodel.FileName = _FileName;  //This is an HTTPPostedFileBase, check if code runs without this
                    productmodel.Product_Name = newproduct.Product.;
                    file.SaveAs(_path);

                }
                using (PiedPiperINEntities productdb = new PiedPiperINEntities())
                {
                    productdb.product.Add(productmodel);
                    productdb.SaveChanges();
                }
                ViewBag.Message = "File Uploaded Successfully!!";
                return View();
                
            }


           
         }

        [HttpGet]
        public ActionResult EditProduct()
        {
            //prod
            //var prod = productdb.product.Where(p => p.Product_ID == Convert.ToInt32(id));

            return View();
        }

        [HttpPost]
        public ActionResult EditProduct(DashboardViewModel passedproduct)
        {
            PiedPiperINEntities productdb = new PiedPiperINEntities();
            

            //product toeditproduct = new product();
            
            //if (pid != null)
            //{
            //    toeditproduct = productdb.product.Where(k => k.Product_ID == pid))
            //    productdb.product.Attach()
            //    if (result != null)
            //    {
            //        pro.Product_Price = "Some new value";
            //        db.SaveChanges();
            //    }
            //}
            return View();
        }
    }
}