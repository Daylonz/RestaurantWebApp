using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using RestaurantWebApp.DAL;
using RestaurantWebApp.Models;

namespace RestaurantWebApp.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private RestaurantContext db = new RestaurantContext();

        public ActionResult Index()
        {
            return View(db.Orders.ToList());
        }

        // GET: Order/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Order/Create
        public ActionResult Create()
        {
            var menuItems = db.MenuItems.ToList();
            var menuItemAmounts = new List<MenuItemAmount>();
            foreach (MenuItem item in menuItems)
            {
                var temp = new MenuItemAmount
                {
                    MenuItem = item,
                    Amount = 0
                };
                menuItemAmounts.Add(temp);
            }

            var discountSelectors = new List<DiscountSelector>();
            foreach (var discount in db.Discounts.ToList())
            {
                var temp = new DiscountSelector
                {
                    Discount = discount,
                    isActive = false
                };
                discountSelectors.Add(temp);
            }

            var orderMenuItems = new CreateOrderViewModel
            {
                MenuItems = menuItemAmounts,
                Discounts = discountSelectors,
                Taxes = db.Taxes.ToList()
            };
            return View(orderMenuItems);
        }

        // POST: Order/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateOrderViewModel orderItems)
        {
            if (ModelState.IsValid)
            {
                //check to ensure there is at least one item
                if (orderItems.MenuItems.All(x => x.Amount <= 0))
                {
                    ModelState.AddModelError("error_message", "You must choose at least one menu item!");
                    return View(orderItems);
                }

                //check to ensure there is no negative menu item
                if (orderItems.MenuItems.Any(x => x.Amount < 0))
                {
                    ModelState.AddModelError("error_message", "You cannot have a negative amount of an item!");
                    return View(orderItems);
                }

                //check for multiple discounts
                if (orderItems.Discounts.FindAll(discount => discount.isActive).Count > 1)
                {
                    ModelState.AddModelError("error_message", "You may only choose one discount per order!");
                    return View(orderItems);
                }

                //get user signed in
                ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                ApplicationUser user = userManager.FindByName(User.Identity.Name);

                //remove all empty items
                orderItems.MenuItems.RemoveAll(item => item.Amount == 0);

                //create list of db items for each item in the order
                var orderMenuItems = new List<OrderMenuItem>();
                foreach (var item in orderItems.MenuItems)
                {
                    var menuItem = db.MenuItems.Where(m => m.Id == item.MenuItem.Id).FirstOrDefault();
                    var ordermenuitem = new OrderMenuItem
                    {
                        MenuItem = menuItem,
                        Amount = item.Amount
                    };
                    orderMenuItems.Add(ordermenuitem);
                }

                //calculate subtotal
                decimal subtotal = 0;
                foreach (var item in orderItems.MenuItems)
                {
                    subtotal += (item.MenuItem.Price * item.Amount);
                }

                //apply discount
                decimal preTaxTotal = subtotal;
                foreach (var discount in orderItems.Discounts)
                {
                    if (discount.isActive)
                    {
                        if (discount.Discount.DiscountType == DiscountType.Fixed)
                        {
                            preTaxTotal -= (decimal)discount.Discount.FlatAmount;
                        }
                        else
                        {
                            preTaxTotal -= (preTaxTotal * (decimal)(discount.Discount.Percent * 0.01m));
                        }
                    }
                }

                decimal discountAmount = subtotal - preTaxTotal;

                //calculate tax amount
                var taxes = db.Taxes.ToList();
                decimal taxAmount = 0;
                int taxpercent = 0;
                foreach (var tax in taxes)
                {
                    taxpercent += tax.Percentage;
                }
                taxAmount = preTaxTotal * (taxpercent * 0.01m);

                //calculate final total
                decimal total = preTaxTotal;
                total += taxAmount;

                //create the rest of the order info
                var order = new Order
                {
                    TimePlaced = DateTime.Now,
                    ServerName = user.FirstName + " " + user.LastName,
                    SubTotal = subtotal,
                    DiscountAmount = discountAmount,
                    PreTaxTotal = preTaxTotal,
                    TaxAmount = taxAmount,
                    Total = total
                };

                //add order info to each order menu item and add to database
                foreach (var item in orderMenuItems)
                {
                    item.Order = order;
                    db.OrderMenuItems.Add(item);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(orderItems);
        }

        // GET: Order/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TimePlaced,ServerName,SubTotal,DiscountAmount,PreTaxTotal,TaxAmount,Total")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: Order/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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
