using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantWebApp.Models
{
    public class CreateViewModel
    {
        [Required]
        public string Name { get; set; }

        [Display(Name = "Discount Type")]
        [Required]
        public DiscountType DiscountType { get; set; }

        [Display(Name = "Fixed Discount Amount")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        [Range(1, 100)]
        public double FlatAmount { get; set; }

        [Display(Name = "Percent Discount Amount")]
        [Range(1, 100)]
        public int Percent { get; set; }
    }
}