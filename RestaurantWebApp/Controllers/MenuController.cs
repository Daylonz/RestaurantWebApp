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
    public class MenuController : Controller
    {
        private RestaurantContext db = new RestaurantContext();
        Regex alphaNumericRegex = new Regex("^[a-zA-Z0-9 ]*$");

        // GET: Menu
        public ActionResult Index()
        {
            return View(db.MenuItems.ToList());
        }

        // GET: Menu/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuItem menuItem = db.MenuItems.Find(id);
            if (menuItem == null)
            {
                return HttpNotFound();
            }
            return View(menuItem);
        }

        // GET: Menu/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Menu/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Category,Name,Price")] MenuItem menuItem)
        {
            if (ModelState.IsValid)
            {
                if (!alphaNumericRegex.IsMatch(menuItem.Name))
                {
                    ModelState.AddModelError("error_message", "Menu item names may only include letters and numbers!");
                    return View(menuItem);
                }
                if (db.MenuItems.Any(d => d.Name.Equals(menuItem.Name)))
                {
                    ModelState.AddModelError("error_message", "Menu item names must be unique!");
                    return View(menuItem);
                }
                db.MenuItems.Add(menuItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(menuItem);
        }

        // GET: Menu/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuItem menuItem = db.MenuItems.Find(id);
            if (menuItem == null)
            {
                return HttpNotFound();
            }
            return View(menuItem);
        }

        // POST: Menu/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Category,Name,Price")] MenuItem menuItem)
        {
            if (ModelState.IsValid)
            {
                if (!alphaNumericRegex.IsMatch(menuItem.Name))
                {
                    ModelState.AddModelError("error_message", "Menu item names may only include letters and numbers!");
                    return View(menuItem);
                }
                if (db.MenuItems.Any(d => d.Name.Equals(menuItem.Name)))
                {
                    ModelState.AddModelError("error_message", "Menu item names must be unique!");
                    return View(menuItem);
                }
                db.Entry(menuItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(menuItem);
        }

        // GET: Menu/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuItem menuItem = db.MenuItems.Find(id);
            if (menuItem == null)
            {
                return HttpNotFound();
            }
            return View(menuItem);
        }

        // POST: Menu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MenuItem menuItem = db.MenuItems.Find(id);
            db.MenuItems.Remove(menuItem);
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
