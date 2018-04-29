﻿using System;
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
    public class SaleController : Controller
    {
        private ShopContext db = new ShopContext();

        // GET: Sale
        public ActionResult Index()
        {
            var storehouseItems = db.StorehouseItems.Include(s => s.Consignment).Include(s => s.Storehouse);
            return View(storehouseItems.ToList());
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

        // GET: Sale/Create
        public ActionResult Create()
        {
            ViewBag.ConsignmentId = new SelectList(db.Consignments, "ConsignmentId", "ConsignmentId");
            ViewBag.StorehouseId = new SelectList(db.Storehouses, "StorehouseId", "StorehouseId");
            return View();
        }

        // POST: Sale/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Price,Quantity,StorehouseId,StorehouseItemId,ConsignmentId")] StorehouseItem storehouseItem)
        {
            if (ModelState.IsValid)
            {
                db.StorehouseItems.Add(storehouseItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ConsignmentId = new SelectList(db.Consignments, "ConsignmentId", "ConsignmentId", storehouseItem.ConsignmentId);
            ViewBag.StorehouseId = new SelectList(db.Storehouses, "StorehouseId", "StorehouseId", storehouseItem.StorehouseId);
            return View(storehouseItem);
        }

        // GET: Sale/Edit/5
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

        // GET: Sale/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Sale/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StorehouseItem storehouseItem = db.StorehouseItems.Find(id);
            db.StorehouseItems.Remove(storehouseItem);
            db.SaveChanges();
            return RedirectToAction("Index");
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