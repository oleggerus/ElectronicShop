using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ElectronicShop;

namespace ElectronicShop.Controllers
{
    public class ChecksController : Controller
    {
        private ShopContext db = new ShopContext();

        // GET: Checks
        public ActionResult Index()
        {
            var checks = db.Checks.Include(c => c.Customer).Include(c => c.Employee);
            return View(checks.ToList());
        }

        // GET: Checks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Check check = db.Checks.Find(id);
            if (check == null)
            {
                return HttpNotFound();
            }
            return View(check);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
