namespace RestaurantWebApp.Migrations
{
    using RestaurantWebApp.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<RestaurantWebApp.DAL.RestaurantContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(RestaurantWebApp.DAL.RestaurantContext context)
        {
            var menuItems = new List<MenuItem>
            {
                new MenuItem {
                    Name = "Fried Calamari",
                    Price = 5.99m,
                    Category = Category.Appetizers
                },
                new MenuItem {
                    Name = "Cheeseburger",
                    Price = 3.99m,
                    Category = Category.Food
                },
                new MenuItem {
                    Name = "Fries",
                    Price = 1.99m,
                    Category = Category.Food
                },
                new MenuItem {
                    Name = "Small Fountain Drink",
                    Price = 1.76m,
                    Category = Category.Drinks
                },
                new MenuItem {
                    Name = "Medium Fountain Drink",
                    Price = 1.98m,
                    Category = Category.Drinks
                },
                new MenuItem {
                    Name = "Large Fountain Drink",
                    Price = 2.38m,
                    Category = Category.Drinks
                }
            };
            menuItems.ForEach(m => context.MenuItems.AddOrUpdate(m));
            context.SaveChanges();


            var orders = new List<Order>
            {
                new Order {
                    TimePlaced = DateTime.Now,
                    ServerName = "Daylon Janis",
                    SubTotal = (menuItems.Single(m => m.Name == "Cheeseburger").Price + menuItems.Single(m => m.Name == "Small Fountain Drink").Price),
                    DiscountAmount = 0m,
                    PreTaxTotal = (menuItems.Single(m => m.Name == "Cheeseburger").Price + menuItems.Single(m => m.Name == "Small Fountain Drink").Price),
                    TaxAmount = ((menuItems.Single(m => m.Name == "Cheeseburger").Price + menuItems.Single(m => m.Name == "Small Fountain Drink").Price) * 0.06m),
                    Total = ((menuItems.Single(m => m.Name == "Cheeseburger").Price + menuItems.Single(m => m.Name == "Small Fountain Drink").Price) + ((menuItems.Single(m => m.Name == "Cheeseburger").Price + menuItems.Single(m => m.Name == "Small Fountain Drink").Price) * 0.06m))
                },
                new Order {
                    TimePlaced = DateTime.Now,
                    ServerName = "Daylon Janis",
                    SubTotal = (menuItems.Single(m => m.Name == "Cheeseburger").Price + menuItems.Single(m => m.Name == "Large Fountain Drink").Price),
                    DiscountAmount = 0m,
                    PreTaxTotal = (menuItems.Single(m => m.Name == "Cheeseburger").Price + menuItems.Single(m => m.Name == "Large Fountain Drink").Price),
                    TaxAmount = ((menuItems.Single(m => m.Name == "Cheeseburger").Price + menuItems.Single(m => m.Name == "Large Fountain Drink").Price) * 0.06m),
                    Total = ((menuItems.Single(m => m.Name == "Cheeseburger").Price + menuItems.Single(m => m.Name == "Large Fountain Drink").Price) + ((menuItems.Single(m => m.Name == "Cheeseburger").Price + menuItems.Single(m => m.Name == "Large Fountain Drink").Price) * 0.06m))
                },
            };
            var ordermenuitems = new List<OrderMenuItem>
            {
                new OrderMenuItem
                {
                    Order = orders[0],
                    MenuItem = menuItems[0],
                    Amount = 1
                },
                new OrderMenuItem
                {
                    Order = orders[0],
                    MenuItem = menuItems[1],
                    Amount = 2
                },
                new OrderMenuItem
                {
                    Order = orders[1],
                    MenuItem = menuItems[0],
                    Amount = 3
                },
                new OrderMenuItem
                {
                    Order = orders[1],
                    MenuItem = menuItems[3],
                    Amount = 1
                }
            };
            ordermenuitems.ForEach(o => context.OrderMenuItems.AddOrUpdate(o));
            context.SaveChanges();

            var discounts = new List<Discount>
            {
                new Discount
                {
                    Name = "5 Dollar Discount",
                    DiscountType = DiscountType.Fixed,
                    FlatAmount = 5.00
                },
                new Discount
                {
                    Name = "5 Percent Discount",
                    DiscountType = DiscountType.Percentage,
                    Percent = 5
                }
            };
            discounts.ForEach(o => context.Discounts.AddOrUpdate(o));
            context.SaveChanges();

            var taxes = new List<Tax>
            {
                new Tax
                {
                    Name = "State Tax",
                    Percentage = 6
                }
            };
            taxes.ForEach(o => context.Taxes.AddOrUpdate(o));
            context.SaveChanges();
        }
    }
}
