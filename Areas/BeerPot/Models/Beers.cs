using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace DatePot.Areas.BeerPot.Models
{
    public class Beers
    {
        public class BeerIndex
        {
            public List<BeerList> BeerList { get; set; }
        }
        public class BeerList
        {
            [Key]
            public int BeerID { get; set; }
            [Display(Name = "Name")]
            public string BeerName { get; set; }
            public string Brewery { get; set; }
            [Display(Name = "Average Wankyness")]
            public int AvgWankyness { get; set; }
            [Display(Name = "Average Taste")]
            public int AvgTaste { get; set; }
            [Display(Name = "Average Rating")]
            public int AvgRating { get; set; }
            public int? UserGroupID { get; set; }
            public List<RandomBeer> RandomBeer { get; set; }
        }
        public class NewBeer
        {
            [Required]
            [MaxLength(100, ErrorMessage = "Names must not exceed 100 characters")]
            [MinLength(2, ErrorMessage = "Names must be at least 2 characters long")]
            [DisplayName("Name")]
            public string BeerName { get; set; }
            public string Brewery { get; set; }
        }
        public class BeerDetails: BeerList
        {
        }
        public class UpdateBeerDetails: BeerDetails
        {
        }
        public class RandomBeer : BeerDetails 
        { 
        }
        public class BeerRatings 
        {
            public int BeerRatingID { get; set; }
            public int BeerID { get; set; }
            public string UserID { get; set; }
            public int Wankyness { get; set; }
            public int Taste { get; set; }
        }
    }
}
