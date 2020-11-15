﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace DatePot.Areas.FoodPot.Models
{
    public class Identity
    {
        public class RestaurantIndex
        {
            public List<RestaurantList> RestaurantList { get; set; }
        }
        public class RestaurantList
        {
            [Key]
            public int RestaurantID { get; set; }
            [Display(Name = "Name")]
            public string RestaurantName { get; set; }
            [Display(Name = "Type")]
            public string FoodTypeText { get; set; }
            [Display(Name = "When")]
            public string WhenText { get; set; }
            public List<RandomRestaurant> RandomRestaurant { get; set; }
        }
        public class UserList
        {
            [Key]
            public int UserID { get; set; }
            [Display(Name = "Name")]
            public string UserName { get; set; }
        }
        public class NewRestaurant
        {
            [Required]
            [MaxLength(100, ErrorMessage = "Names must not exceed 100 characters")]
            [MinLength(2, ErrorMessage = "Names must be at least 2 characters long")]
            [DisplayName("Name")]
            public string RestaurantName { get; set; }
            public List<FoodTypes> FoodTypes { get; set; }
            [Required]
            public List<When> When { get; set; }
            public int FoodTypeID { get; set; }
            public int WhenID { get; set; }
        }
        public class FoodTypes
        {
            public int FoodTypeID { get; set; }
            public string FoodTypeText { get; set; }
        }
        public class When
        {
            public int WhenID { get; set; }
            public string WhenText { get; set; }
        }
        public class RestaurantDetails
        {
            [Key]
            public int RestaurantID { get; set; }
            [Display(Name = "Name")]
            public string RestaurantName { get; set; }
        }
        public class UpdateRestaurantDetails: RestaurantDetails
        {
        }
        public class RandomRestaurant : RestaurantDetails { }
        public class NewFoodType
        {
            [Required]
            [DisplayName("FoodType")]
            public string FoodTypeText { get; set; }
        }
        public class NewWhen
        {
            [Required]
            [DisplayName("When")]
            public string WhenText { get; set; }
        }

    }
}
