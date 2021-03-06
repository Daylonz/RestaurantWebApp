﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using RestaurantWebApp.DAL;
using RestaurantWebApp.Models;

namespace RestaurantWebApp.Controllers
{

    [Authorize]
    public class DiscountController : Controller
    {
        private RestaurantContext db = new RestaurantContext();
        Regex alphaNumericRegex = new Regex("^[a-zA-Z0-9 ]*$");

        // GET: Discount
        public ActionResult Index()
        {
            return View(db.Discounts.ToList());
        }

        // GET: Discount/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discount discount = db.Discounts.Find(id);
            if (discount == null)
            {
                return HttpNotFound();
            }
            return View(discount);
        }

        // GET: Discount/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Discount/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,DiscountType,FlatAmount,Percent")] Discount discount)
        {
            if (ModelState.IsValid)
            {
                if (!alphaNumericRegex.IsMatch(discount.Name))
                {
                    ModelState.AddModelError("error_message", "Discount names may only include letters and numbers!");
                    return View(discount);
                }
                if (db.Discounts.Any(d => d.Name.Equals(discount.Name)))
                {
                    ModelState.AddModelError("error_message", "Discount names must be unique!");
                    return View(discount);
                }

                db.Discounts.Add(discount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(discount);
        }

        // GET: Discount/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discount discount = db.Discounts.Find(id);
            if (discount == null)
            {
                return HttpNotFound();
            }
            return View(discount);
        }

        // POST: Discount/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,DiscountType,FlatAmount,Percent")] Discount discount)
        {
            if (ModelState.IsValid)
            {
                if (!alphaNumericRegex.IsMatch(discount.Name))
                {
                    ModelState.AddModelError("error_message", "Discount names may only include letters and numbers!");
                    return View(discount);
                }
                if (db.Discounts.Any(d => d.Name.Equals(discount.Name)))
                {
                    ModelState.AddModelError("error_message", "Discount names must be unique!");
                    return View(discount);
                }
                db.Entry(discount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(discount);
        }

        // GET: Discount/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discount discount = db.Discounts.Find(id);
            if (discount == null)
            {
                return HttpNotFound();
            }
            return View(discount);
        }

        // POST: Discount/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Discount discount = db.Discounts.Find(id);
            db.Discounts.Remove(discount);
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
