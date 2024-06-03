using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Employee System that has simple CRUD Function";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "My contacts";

            return View();
        }
    }
}