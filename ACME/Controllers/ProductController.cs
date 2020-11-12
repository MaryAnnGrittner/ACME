using ACME.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;
using System.IO;

namespace ACME.Controllers
{
    public class ProductController : Controller
    {
        SqlConnection dbcon = new SqlConnection(
        ConfigurationManager.ConnectionStrings["AcmeDB"].ConnectionString.ToString());
        // GET: Product

        public ActionResult Index2()
        {
            return View();
        }

        public ActionResult Index()
        {
            List<Product> productlist;
            try
            {
                dbcon.Open();
                productlist = Product.GetProductList(dbcon, "");
                dbcon.Close();
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            return View(productlist);
        }

        public ActionResult Edit(string id = "")
        {
            if (Regex.IsMatch(id, @"^[A-Za-z0-9]{2,10}$"))
            {
                try
                {
                    dbcon.Open();
                    Product prod = Product.GetProductSingle(dbcon, id);

                    dbcon.Close();
                    return View(prod);
                }
                catch (Exception ex) { throw new Exception(ex.Message); }
            }
            ViewBag.errormsg = "Invalid data in the Edit Page";
            return View("Error");
        }

        [HttpPost]
        public ActionResult Edit(Product prod, HttpPostedFileBase uploadfile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (uploadfile != null && uploadfile.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(uploadfile.FileName);
                        var path = Path.Combine(
                        Server.MapPath("~/Content/Images/products"), fileName);
                        uploadfile.SaveAs(path);
                        prod.ImageFile = fileName;
                    }

                        dbcon.Open();
                        int intresult = Product.CUDProduct(dbcon, "update", prod);
                        dbcon.Close();
                        return RedirectToAction("Index");
                }
                catch (Exception ex) { throw new Exception(ex.Message); }
            } //valid data
            ViewBag.errmsg = "Data validation error in Edit method";
            return View("Error");


        } 

        public ActionResult Delete(string id = "")
        {
            if (Regex.IsMatch(id, @"^[A-Za-z0-9]{2,10}$"))
            {
                try
                {
                    dbcon.Open();
                    Product prod = Product.GetProductSingle(dbcon, id);

                    dbcon.Close();
                    return View(prod);
                }
                catch (Exception ex) { throw new Exception(ex.Message); }
            }
            ViewBag.errormsg = "Invalid data in the Edit Page";
            return View("Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, FormCollection fc)
        {
            Product prod = new Models.Product();
            prod.ProductID = id;
            try
            {
                dbcon.Open();
                int intresult = Product.CUDProduct(dbcon, "delete", prod);
                dbcon.Close();
                return RedirectToAction("Index");
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

    

        public ActionResult Create()
        {
            Product prod = new Models.Product();
            prod.ImageFile = "nopic.jpg";
            return View(prod);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product prod, HttpPostedFileBase uploadfile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (uploadfile != null && uploadfile.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(uploadfile.FileName);
                        var path = Path.Combine(
                        Server.MapPath("~/Content/Images/products"), fileName);
                        uploadfile.SaveAs(path);
                        prod.ImageFile = fileName;
                    }

                    dbcon.Open();
                    int intresult = Product.CUDProduct(dbcon, "create", prod);
                    dbcon.Close();
                    return RedirectToAction("Index");
                }
                catch (Exception ex) { throw new Exception(ex.Message); }
            }
            ViewBag.errormsg = "Invalid data in Create Post action method";
            return View("Error");
        }


    }
}