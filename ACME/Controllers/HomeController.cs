using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACME.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShowSessions()
        {
            string strx = "Here are the sessions variables:<br>";
            foreach (string s1 in Session.Keys)
            {
                if (Session[s1] != null)
                    strx = strx + s1 + " = " + Session[s1].ToString() + "<br>";
            }
            ViewBag.SessionValues = strx;
            return View();
        }

    }
}