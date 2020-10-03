using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantWebApp.Models
{
    public class CreateOrderViewModel
    {
        public List<MenuItemAmount> MenuItems { get; set; }
        public List<DiscountSelector> Discounts { get; set; }
        public List<Tax> Taxes { get; set; }
    }

    public class MenuItemAmount
    {
        public MenuItem MenuItem { get; set; }
        public int Amount { get; set; }
    }

    public class DiscountSelector
    {
        public Discount Discount { get; set; }
        public Boolean isActive { get; set; }
    }
}