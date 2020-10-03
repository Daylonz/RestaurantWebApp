using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantWebApp.Models
{
    public class Discount
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(25, ErrorMessage = "The {0} must be between 1 and 25 characters long.", MinimumLength = 1)]
        public string Name { get; set; }

        [Display(Name = "Discount Type")]
        [Required]
        public DiscountType DiscountType { get; set; }

        [Display(Name = "Fixed Discount Amount")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        [Range(1, 100)]
        public double? FlatAmount { get; set; }

        [Display(Name = "Percent Discount Amount")]
        [Range(1, 100)]
        public int? Percent { get; set; }
    }
    public enum DiscountType
    {
        Fixed = 0,
        Percentage = 1
    }
}