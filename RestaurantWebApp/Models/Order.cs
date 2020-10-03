using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantWebApp.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Display(Name = "Time Placed")]
        public DateTime TimePlaced { get; set; }

        [Display(Name = "Server Name")]
        public string ServerName { get; set; }

        [Display(Name = "Menu Items")]
        public virtual ICollection<OrderMenuItem> OrderMenuItem { get; set; }

        [Display(Name = "Sub-Total")]
        public decimal SubTotal { get; set; }

        [Display(Name = "Discount Amount")]
        public decimal DiscountAmount { get; set; }

        [Display(Name = "Pre-Tax Total")]
        public decimal PreTaxTotal { get; set; }

        [Display(Name = "Tax Amount")]
        public decimal TaxAmount { get; set; }

        [Display(Name = "Total")]
        public decimal Total { get; set; }
    }
}