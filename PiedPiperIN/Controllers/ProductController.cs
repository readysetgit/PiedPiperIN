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
            ViewBag.Head = "Upload Product";
            return View();
        }
        [HttpPost]
        public ActionResult UploadProduct(product newproduct, HttpPostedFileBase file)
        {
            ViewBag.Head = "Upload Product";
            try
            {
                product productmodel = new product();

                if (file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("/Content/"), _FileName);
                    productmodel.Product_Pic = _FileName;
                    //productmodel.FileName = _FileName;  //This is an HTTPPostedFileBase, check if code runs without this
                    productmodel.Product_Name = newproduct.Product_Name;
                    file.SaveAs(_path);

                }
                using (PiedPiperINEntities productdb = new PiedPiperINEntities())
                {
                    productdb.product.Add(productmodel);
                    productdb.SaveChanges();
                }
                ViewBag.Message = "File Uploaded Successfully!!";
                return View();
                throw new Exception();
            }
            catch (Exception)
            {
                ViewBag.Head = "Something Went Wrong!";
                return View();
            }
        }
    }
}