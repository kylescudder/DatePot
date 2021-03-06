﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace DatePot.Areas.CoffeePot.Models
{
    public class Coffees
    {
        public class CoffeeIndex
        {
            public List<CoffeeList> CoffeeList { get; set; }
        }
        public class CoffeeList
        {
            [Key]
            public int CoffeeID { get; set; }
            [Display(Name = "Name")]
            public string CoffeeName { get; set; }
            [Display(Name = "Average Taste")]
            public int AvgTaste { get; set; }
            [Display(Name = "Average Rating")]
            public int AvgRating { get; set; }
            public int? UserGroupID { get; set; }
            public List<RandomCoffee> RandomCoffee { get; set; }
        }
        public class NewCoffee
        {
            [Required]
            [MaxLength(100, ErrorMessage = "Names must not exceed 100 characters")]
            [MinLength(2, ErrorMessage = "Names must be at least 2 characters long")]
            [DisplayName("Name")]
            public string CoffeeName { get; set; }
        }
        public class CoffeeDetails: CoffeeList
        {
        }
        public class UpdateCoffeeDetails: CoffeeDetails
        {
        }
        public class RandomCoffee : CoffeeDetails { }
        public class CoffeeRatings 
        {
            public int CoffeeRatingID { get; set; }
            public int CoffeeID { get; set; }
            public string UserID { get; set; }
            public int Experience { get; set; }
            public int Taste { get; set; }
        }
    }
}
