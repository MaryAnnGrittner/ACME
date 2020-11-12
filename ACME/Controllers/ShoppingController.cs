using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ACME.Models;
using ACME.Models.ViewModels;

namespace ACME.Controllers
{
    public class ShoppingController : Controller
    {
        SqlConnection dbcon = new SqlConnection(
        ConfigurationManager.ConnectionStrings["AcmeDB"].ConnectionString.ToString());

        // GET: Shopping
        public ActionResult Index(string id = "")
        {
            if (Regex.IsMatch(id, @"^[A-Za-z0-9]{2,10}$"))
            {
                //try...catch block
                List<Product> productlist;
                try
                {
                    ViewBag.Categoryid = id;
                    dbcon.Open();
                    productlist = Product.GetProductList(dbcon, "where categoryid = '" + id + "' ");
                    dbcon.Close();
                }
                catch (Exception ex) { throw new Exception(ex.Message); }
                return View(productlist);
            }
            ViewBag.errormsg = "Invalid data found in Shopping Page";
            return View("error");
        }
        public ActionResult Order(string id = "")
        {
            if (Regex.IsMatch(id, @"^[A-Za-z0-9]{2,10}$"))
            {
                //try...catch block
                Product product = new Models.Product();
                try
                {
                    dbcon.Open();
                    product = Product.GetProductSingle(dbcon, id);
                    dbcon.Close();
                }
                catch (Exception ex) { throw new Exception(ex.Message); }
                return View(product);
            }
            ViewBag.errormsg = "Invalid data found in Shopping Page";
            return View("error");
        }
        [HttpPost]
        public ActionResult Order(Cart_Lineitem cart)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    dbcon.Open();
                    if (Session["cartnumber"] == null)
                        Session["cartnumber"] = Utility.GetIdNumber(dbcon, "CartNumber");
                    int cartnumber = (int)Session["cartnumber"];
                    cart.CartNumber = cartnumber;
                    int intresult = Cart_Lineitem.CartUpSert(dbcon, cart);
                    dbcon.Close();
                    return RedirectToAction("Cart");
                }
                catch (Exception ex) { throw new Exception(ex.Message); }
            }
            ViewBag.errormsg = "Invalid data found in Order Page";
            return View("error");
        }
        [HttpPost]
        public ActionResult Cart(Cart_Lineitem cart, string udaction="")
        {
            udaction = udaction.ToLower();
            if (ModelState.IsValid && (udaction == "update" || udaction == "delete"))
            {
                try
                {
                    dbcon.Open();
                    //cart.CartNumber = 100;
                    int cartnumber = (int)Session["cartnumber"];
                    cart.CartNumber = cartnumber;
                    int intresult = Cart_Lineitem.CUDCartLineitem(dbcon, udaction, cart);
                    dbcon.Close();
                    return RedirectToAction("Cart");
                }
                catch (Exception ex) { throw new Exception(ex.Message); }
            }
            ViewBag.errormsg = "Invalid data found in Order Page";
            return View("error");
        }
        public ActionResult Cart()
        {
            List<Cartvm1> cartlist = new List<Cartvm1>();
            try
            {
                if (Session["cartnumber"] != null)
                {
                    dbcon.Open();
                    int cartnumber = (int)Session["cartnumber"];
                    cartlist = Cartvm1.GetCartList(dbcon, cartnumber);
                    dbcon.Close();
                }
                return View(cartlist);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}