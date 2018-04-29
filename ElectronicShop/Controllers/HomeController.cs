using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ElectronicShop.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Employee user, string returnUrl)
        {
            var db = new ShopContext();

            var dataItem = db.Employees.FirstOrDefault(x => x.Surname == user.Surname
                                                   && x.Name == user.Name
                                                   && x.Password == user.Password);
            if (dataItem != null)
            {
                FormsAuthentication.SetAuthCookie((dataItem.EmployeeId).ToString(), false);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Некоректні дані. Спробуйте ще раз.");
            return View();
        }

        [Authorize]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }
    }
}