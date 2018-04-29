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
            ShopContext db = new ShopContext();

            var dataItem = db.Employees.First(x => x.Surname == user.Surname
                                                   && x.Name == user.Name
                                                   && x.Password == user.Password);
            if (dataItem != null)
            {
                FormsAuthentication.SetAuthCookie((dataItem.EmployeeId).ToString(), false);
                if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                    && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                {
                    return Redirect(returnUrl);
                }

                else
                {
                    return RedirectToAction("Index");
                }
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