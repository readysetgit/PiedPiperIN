using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PiedPiperIN.Models;

//
namespace PiedPiperIN.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        [HttpPost]
        public ActionResult FillUploadBox(string pid, string pcategory, string pname, string pprice, string ppic)
        {
            PiedPiperINEntities productdb = new PiedPiperINEntities();
            DashboardViewModel dash = new DashboardViewModel();

            dash.Product = productdb.products.ToList();
            dash.Cart = productdb.cart_view.ToList();

            ViewBag.name = pname;
            ViewBag.id = pid;
            ViewBag.Price = pprice;
            ViewBag.category = pcategory;

            return View("UploadProduct", dash);
        }




        [HttpGet]
        [Authorize(Users = "admin")]
        public ActionResult UploadProduct()
        {
            PiedPiperINEntities productdb = new PiedPiperINEntities();
            DashboardViewModel dash = new DashboardViewModel();

            dash.Product = productdb.products.ToList();
            dash.Cart = productdb.cart_view.ToList();

            ViewBag.Head = "Upload Product";
            return View(dash);

        }


        [HttpPost]
        public ActionResult NewProduct(product newproduct, HttpPostedFileBase file)
        {
            DashboardViewModel dash = new DashboardViewModel();
            PiedPiperINEntities productdb = new PiedPiperINEntities();
            product productmodel = new product();

            var exists = productdb.products.Find(newproduct.Product_ID);
            if (exists == null)
            {
                if (file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("/Content/"), _FileName);
                    productmodel.Product_Pic = _FileName;
                    //productmodel.FileName = _FileName;  //This is an HTTPPostedFileBase, check if code runs without this
                    productmodel.Product_Name = newproduct.Product_Name;
                    productmodel.Product_Price = newproduct.Product_Price;
                    productmodel.Product_category = (int)newproduct.Product_category;

                    file.SaveAs(_path);

                }




                productdb.products.Add(productmodel);
                productdb.SaveChanges();

            }
            else
            {
                if (file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("/Content/"), _FileName);
                    int pro_id = Convert.ToInt32(newproduct.Product_ID);
                    productmodel = productdb.products.FirstOrDefault(m => m.Product_ID == pro_id);
                    productmodel.Product_Pic = _FileName;
                    //productmodel.FileName = _FileName;  //This is an HTTPPostedFileBase, check if code runs without this
                    productmodel.Product_Name = newproduct.Product_Name;
                    productmodel.Product_Price = newproduct.Product_Price;
                    productmodel.Product_category = newproduct.Product_category;
                    file.SaveAs(_path);
                    productdb.SaveChanges();
                }



                

            }
            ViewBag.Message = "File Uploaded Successfully!!";
            dash.Product = productdb.products.ToList();
            dash.Cart = productdb.cart_view.ToList();
            ViewBag.name = "Product Name";
            ViewBag.id = null;
            ViewBag.Price = "Product Price";
            return View("UploadProduct", dash);




        }

        [HttpPost]
        public ActionResult DeleteProduct(string pid)
        {
            PiedPiperINEntities productdb = new PiedPiperINEntities();
            DashboardViewModel dash = new DashboardViewModel();
            int pro_id = Convert.ToInt32(pid);
            product productdelete = new product();
            productdelete = productdb.products.FirstOrDefault(m => m.Product_ID == pro_id);
            if (productdelete != null)
            {
                productdb.products.Remove(productdelete);
                productdb.SaveChanges();
            }


            dash.Product = productdb.products.ToList();
            dash.Cart = productdb.cart_view.ToList();
            return View("UploadProduct", dash);
        }
        
    }
}
