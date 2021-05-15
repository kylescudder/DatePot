using System;
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
            [Display(Name = "Kyle Taste")]
            public int KyleTaste { get; set; }
            [Display(Name = "Rhiann Taste")]
            public int RhiannTaste { get; set; }
            [Display(Name = "Kyle Experience")]
            public int KyleExperience { get; set; }
            [Display(Name = "Rhiann Experience")]
            public int RhiannExperience { get; set; }
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
    }
}
