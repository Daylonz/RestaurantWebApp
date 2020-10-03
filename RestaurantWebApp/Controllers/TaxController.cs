using System;
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
    public class TaxController : Controller
    {
        private RestaurantContext db = new RestaurantContext();
        Regex alphaNumericRegex = new Regex("^[a-zA-Z0-9 ]*$");

        // GET: Tax
        public ActionResult Index()
        {
            return View(db.Taxes.ToList());
        }

        // GET: Tax/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tax tax = db.Taxes.Find(id);
            if (tax == null)
            {
                return HttpNotFound();
            }
            return View(tax);
        }

        // GET: Tax/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tax/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Percentage")] Tax tax)
        {
            if (ModelState.IsValid)
            {
                if (!alphaNumericRegex.IsMatch(tax.Name))
                {
                    ModelState.AddModelError("error_message", "Tax names may only include letters and numbers!");
                    return View(tax);
                }
                if (db.Taxes.Any(t => t.Name.Equals(tax.Name)))
                {
                    ModelState.AddModelError("error_message", "Tax names must be unique!");
                    return View(tax);
                }
                db.Taxes.Add(tax);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tax);
        }

        // GET: Tax/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tax tax = db.Taxes.Find(id);
            if (tax == null)
            {
                return HttpNotFound();
            }
            return View(tax);
        }

        // POST: Tax/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Percentage")] Tax tax)
        {
            if (ModelState.IsValid)
            {
                if (!alphaNumericRegex.IsMatch(tax.Name))
                {
                    ModelState.AddModelError("error_message", "Tax names may only include letters and numbers!");
                    return View(tax);
                }
                if (db.Taxes.Any(t => t.Name.Equals(tax.Name)))
                {
                    ModelState.AddModelError("error_message", "Tax names must be unique!");
                    return View(tax);
                }
                db.Entry(tax).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tax);
        }

        // GET: Tax/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tax tax = db.Taxes.Find(id);
            if (tax == null)
            {
                return HttpNotFound();
            }
            return View(tax);
        }

        // POST: Tax/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tax tax = db.Taxes.Find(id);
            db.Taxes.Remove(tax);
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
