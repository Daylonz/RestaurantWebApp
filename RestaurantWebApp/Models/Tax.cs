using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantWebApp.Models
{
    public class Tax
    {
        public int Id { get; set; }

        [StringLength(25, ErrorMessage = "The {0} must be between 1 and 25 characters long.", MinimumLength = 1)]
        public string Name { get; set; }
        public int Percentage { get; set; }
    }
}