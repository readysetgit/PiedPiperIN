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
        public ActionResult NewProduct(product newproduct, HttpPostedFileBase file)
        {
            { 
            DashboardViewModel dash = new DashboardViewModel();
            PiedPiperINEntities productdb = new PiedPiperINEntities();
                product productmodel = new product();
                var existingproduct = productdb.product.Find(newproduct.Product_ID);    
                
                
                    

                    if (file.ContentLength > 0)
                    {
                        string _FileName = Path.GetFileName(file.FileName);
                        string _path = Path.Combine(Server.MapPath("/Content/"), _FileName);
                        productmodel.Product_Pic = _FileName;
                        //productmodel.FileName = _FileName;  //This is an HTTPPostedFileBase, check if code runs without this
                        productmodel.Product_Name = newproduct.Product_Name;
                        productmodel.Product_Price = newproduct.Product_Price;
                        file.SaveAs(_path);

                    }

                if (existingproduct == null)
                     {
                        productdb.product.Add(productmodel);
                        productdb.SaveChanges();
                    }
                else
                {
                    productdb.Entry(existingproduct).CurrentValues.SetValues(productmodel);
                }
                    ViewBag.Message = "File Uploaded Successfully!!";
                    dash.Product = productdb.product.ToList();
                    dash.Cart = productdb.cart_view.ToList();
                    return View("UploadProduct", dash);
                
              
            }


           
         }

        //[HttpGet]
        //public ActionResult EditProduct()
        //{
        //    //prod
        //    //var prod = productdb.product.Where(p => p.Product_ID == Convert.ToInt32(id));

        //    return View();
        //}

        //[HttpPost]
        //public actionresult editproduct(string pid, string name, string price, string picpath)
        //{
        //    piedpiperinentities productdb = new piedpiperinentities();
        //    product editedproduct = new product();
        //    editedproduct =

        //    product toeditproduct = new product();

        //    if (pid != null)
        //    {
        //        toeditproduct = productdb.product.where(k => k.product_id == pid))
        //        productdb.product.attach()
        //        if (result != null)
        //        {
        //            pro.product_price = "some new value";
        //            db.savechanges();
        //        }
        //    }
        //    return view();
        //}
    }
}