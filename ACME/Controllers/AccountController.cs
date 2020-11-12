using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACME.Models.ViewModels;
using System.Web.Security;
using ACME.Models;
using System.Data.SqlClient;
using System.Configuration;

namespace ACME.Controllers
{
    public class AccountController : Controller
    {
        SqlConnection dbcon = new SqlConnection(
        ConfigurationManager.ConnectionStrings["AcmeDB"].ConnectionString.ToString());

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (Request.QueryString["returnurl"] == null)
                return RedirectToAction("Index", "Home");
            Loginvm loginvm = new Loginvm();
            loginvm.Username = "wirthw@matc.edu";
            loginvm.Password = "qwerty";
            ViewBag.message = "";
            return View(loginvm);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Login(Loginvm login)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    dbcon.Open();
                    Customer cust = Customer.GetCustomerSingle(dbcon, 0, login.Username);
                    dbcon.Close();
                    if (cust.CustNumber > 0 && cust.PWD == login.Password)
                    {
                        string ReturnUrl = Request.QueryString["returnurl"].ToString();
                        if (ReturnUrl.Length > 1 && Url.IsLocalUrl(ReturnUrl))
                        {
                            Session["custid"] = cust.CustNumber;
                            FormsAuthentication.SetAuthCookie(login.Username, false);
                            return Redirect(ReturnUrl);
                        }
                        else
                            return RedirectToAction("Index", "Home");
                    }
                }
                catch (Exception ex) { throw new Exception(ex.Message); }
            }
            ViewBag.message = "Credentials are not valid";
            return View(login);
        }
    }
}