using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ElectronicShop;
using WebGrease.Css.Extensions;

namespace ElectronicShop.Controllers
{
    public class SaleController : Controller
    {
        private ShopContext db = new ShopContext();

        [Authorize]
        public ActionResult Index(string sortOrder, string searchString)
        {
            ViewBag.CategorySortParm = String.IsNullOrEmpty(sortOrder) ? "Category_desc" : "";
            ViewBag.ProducerSortParm = sortOrder == "Producer" ? "Producer_desc" : "Producer";
            ViewBag.ModelSortParm = sortOrder == "Model" ? "Model_desc" : "Model";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "Price_desc" : "Price";
            ViewBag.QuantitySortParm = sortOrder == "Quantity" ? "Quantity_desc" : "Quantity";
            ViewBag.ReservSortParm = sortOrder == "Reserv" ? "Reserv_desc" : "Reserv";

            var idEmp = Convert.ToInt32(HttpContext.User.Identity.Name);

            var storehouseItems = (from s in db.StorehouseItems
                                   join c in db.Consignments on s.ConsignmentId equals c.ConsignmentId
                                   join st in db.Storehouses on s.StorehouseId equals st.StorehouseId
                                   join stEmp in db.StorehouseEmployees on st.StorehouseId equals stEmp.StorehouseId
                                   join emp in db.Employees on stEmp.EmployeeId equals emp.EmployeeId
                                   where emp.EmployeeId == idEmp
                                   select s);
            if (!String.IsNullOrEmpty(searchString))
            {
                storehouseItems = storehouseItems.Where(s => s.Consignment.Item.Producer.Name.Contains(searchString)
                                               || s.Consignment.Item.Model.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "Category_desc":
                    storehouseItems = storehouseItems.OrderByDescending(s => s.Consignment.Item.Category.Name);
                    break;
                case "Producer":
                    storehouseItems = storehouseItems.OrderBy(s => s.Consignment.Item.Producer.Name);
                    break;
                case "Producer_desc":
                    storehouseItems = storehouseItems.OrderByDescending(s => s.Consignment.Item.Producer.Name);
                    break;
                case "Price":
                    storehouseItems = storehouseItems.OrderBy(s => s.Price);
                    break;
                case "Price_desc":
                    storehouseItems = storehouseItems.OrderByDescending(s => s.Price);
                    break;
                case "Model":
                    storehouseItems = storehouseItems.OrderBy(s => s.Consignment.Item.Model);
                    break;
                case "Model_desc":
                    storehouseItems = storehouseItems.OrderByDescending(s => s.Consignment.Item.Model);
                    break;
                case "Quantity":
                    storehouseItems = storehouseItems.OrderBy(s => s.Quantity);
                    break;
                case "Quantity_desc":
                    storehouseItems = storehouseItems.OrderByDescending(s => s.Quantity);
                    break;
                default:
                    storehouseItems = storehouseItems.OrderBy(s => s.Consignment.Item.Category.Name);
                    break;
            }
            return View(storehouseItems.ToList());
        }

        [HttpPost]
        public ActionResult Index(List<string> quantities, string name, string surname)
        {
            var customer = db.Customers.FirstOrDefault(x => x.Name == name
                                                                  && x.Surname == surname);

            var idEmp = Convert.ToInt32(HttpContext.User.Identity.Name);
            var items = quantities.ToArray();
            int counter = 0;
            var storehouseItems = (from s in db.StorehouseItems
                                   join c in db.Consignments on s.ConsignmentId equals c.ConsignmentId
                                   join st in db.Storehouses on s.StorehouseId equals st.StorehouseId
                                   join stEmp in db.StorehouseEmployees on st.StorehouseId equals stEmp.StorehouseId
                                   join emp in db.Employees on stEmp.EmployeeId equals emp.EmployeeId
                                   where emp.EmployeeId == idEmp
                                   orderby s.Consignment.Item.CategoryId
                                   select s).ToArray();



            var theDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, DateTime.Today.Hour, DateTime.Today.Minute, DateTime.Today.Second); ;
            double total = 0;

            var check = new Check
            {
                EmployeeId = idEmp,
                CheckDate = theDate,
                TotalPrice = 1,
                Customer = customer
            };
            db.Checks.Add(check);

            foreach (var item in items)
            {
                if (int.TryParse(items[counter], out var amount))
                {
                    if (amount > 0)
                    {
                        //var discounts = (from d in db.DiscountToItems
                        //                 join ds in db.Discounts on d.DiscountId equals ds.DiscountId
                        //                 where storehouseItems[counter].StorehouseItemId == d.StorehouseItemId
                        //                 select ds.DiscountValue);

                        //double sum_disc = 0;
                        //discounts.ForEach(x => sum_disc += x);
                        //foreach (var discount in discounts)
                        //{
                        //    sum_disc += discount;
                        //    if (sum_disc > 50)
                        //    {
                        //        sum_disc = 50; break;
                        //    }
                        //}

                        var sale = new Sale
                        {
                            Quantity = amount,
                            StorehouseItemId = storehouseItems[counter].StorehouseItemId,
                            SaleDate = theDate,
                            CheckId = check.CheckId
                        };
                        total += Convert.ToInt32(storehouseItems[counter].Price) * amount; //-Convert.ToInt32(storehouseItems[counter].Price) * amount/100*(int)sum_disc;
                        db.Sales.Add(sale);
                        var storehouseItem = db.StorehouseItems
                            .FirstOrDefault(s => s.StorehouseItemId == sale.StorehouseItemId);
                        if (storehouseItem.Quantity >= sale.Quantity)
                        {
                            storehouseItem.Quantity -= sale.Quantity;
                        }
                        else
                        {
                            var error = "Для " + storehouseItem.Consignment.Item.Producer.Name + " " +
                                        storehouseItem.Consignment.Item.Model + " не вистачає товару";

                            items[0] = "";
                            quantities.ToArray();
                            quantities[0] = "";
                            ViewBag.ErrorMessage = error;
                            return View(storehouseItems);
                        }
                    }
                }
                else if (items[counter] != "" && items[counter] != "0")
                {
                    items[0] = "";
                    quantities.ToArray();
                    quantities[0] = "";
                    ViewBag.ErrorMessage = "Некоректні дані";
                    return View(storehouseItems);
                }

                counter++;
            }

            if (total == 0)
            {
                ViewBag.ErrorMessage = "Товари не вибрано";
                return View(storehouseItems);
            }

            if (customer?.CustomerDiscount != null)
            {
                if (customer.CustomerDiscount.DiscountValue > 0)
                {
                    check.TotalPrice = total - total / 100 * customer.CustomerDiscount.DiscountValue;
                }
            }
            else
            {
                check.TotalPrice = (double)total;
            }
            db.Checks.AddOrUpdate(check);
            db.SaveChanges();
            ViewBag.Message = "Покупка успішно здійснена";
            items[0] = "";
            quantities.ToArray();
            quantities[0] = "";
            return View(storehouseItems);
        }

        // GET: Sale/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StorehouseItem storehouseItem = db.StorehouseItems.Find(id);
            if (storehouseItem == null)
            {
                return HttpNotFound();
            }
            return View(storehouseItem);
        }


        public ActionResult CreateRequest(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StorehouseItem storehouseItem = db.StorehouseItems.Find(id);
            if (storehouseItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.ConsignmentId = new SelectList(db.Consignments, "ConsignmentId", "ConsignmentId", storehouseItem.ConsignmentId);
            ViewBag.StorehouseId = new SelectList(db.Storehouses, "StorehouseId", "StorehouseId", storehouseItem.StorehouseId);
            return View(storehouseItem);
        }

        // POST: Sale/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public void CreateRequest([Bind(Include = "Price,Quantity,StorehouseId,StorehouseItemId,ConsignmentId")] StorehouseItem storehouseItem, int quantities)
        {
            var st = db.StorehouseItems.FirstOrDefault(x => x.StorehouseItemId == storehouseItem.StorehouseItemId);
            if (st.Storehouse.IsShop == true)
            {
                //var st = db.StorehouseItems.Where(x => x.Consignment.ItemId == storehouseItem.Consignment.ItemId)
                //    .Where(x => x.Storehouse.IsShop == false);
                //var max = st.Max(x => x.Quantity);

                //if (max >= quantities)
                //{
                //}


                st.Quantity += quantities;
                db.StorehouseItems.AddOrUpdate(st);
                db.SaveChanges();
            }

            RedirectToAction("Index", "Home");

        }

        [Authorize(Roles = "Manager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StorehouseItem storehouseItem = db.StorehouseItems.Find(id);
            if (storehouseItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.ConsignmentId = new SelectList(db.Consignments, "ConsignmentId", "ConsignmentId", storehouseItem.ConsignmentId);
            ViewBag.StorehouseId = new SelectList(db.Storehouses, "StorehouseId", "StorehouseId", storehouseItem.StorehouseId);
            return View(storehouseItem);
        }

        // POST: Sale/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Price,Quantity,StorehouseId,StorehouseItemId,ConsignmentId")] StorehouseItem storehouseItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(storehouseItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ConsignmentId = new SelectList(db.Consignments, "ConsignmentId", "ConsignmentId", storehouseItem.ConsignmentId);
            ViewBag.StorehouseId = new SelectList(db.Storehouses, "StorehouseId", "StorehouseId", storehouseItem.StorehouseId);
            return View(storehouseItem);
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
