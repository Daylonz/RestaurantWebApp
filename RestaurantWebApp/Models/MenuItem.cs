using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantWebApp.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public Category Category { get; set; }

        [StringLength(25, ErrorMessage = "The {0} must be between 1 and 25 characters long.", MinimumLength = 1)]
        public string Name { get; set; }

        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        [Range(0, 100)]
        public decimal Price { get; set; }

        public virtual ICollection<OrderMenuItem> OrderMenuItem { get; set; }
    }

    public enum Category
    {
        Appetizers = 0,
        Food = 1,
        Drinks = 2
    }
}