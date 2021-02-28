using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace DatePot.Areas.VinylPot.Models
{
    public class Vinyls
    {
        public class VinylIndex
        {
            public List<VinylList> VinylList { get; set; }
        }
        public class VinylList
        {
            [Key]
            public int VinylID { get; set; }
            [Display(Name = "Name")]
            public string Name { get; set; }
            [Display(Name = "Artist")]
            public string ArtistName { get; set; }
            public string Purchased { get; set; }
        }
        public class NewVinyl
        {
            [Required]
            [MaxLength(100, ErrorMessage = "Names must not exceed 100 characters")]
            [MinLength(2, ErrorMessage = "Names must be at least 2 characters long")]
            [DisplayName("Name")]
            public string Name { get; set; }
            [Required]
            [MaxLength(100, ErrorMessage = "Names must not exceed 100 characters")]
            [MinLength(2, ErrorMessage = "Names must be at least 2 characters long")]
            [DisplayName("Artist Name")]
            public string ArtistName { get; set; }
            public bool Purchased { get; set; }
        }
        public class VinylDetails
        {
            [Key]
            public int VinylID { get; set; }
            [Display(Name = "Name")]
            public string Name { get; set; }
            [Display(Name = "Artist")]
            public string ArtistName { get; set; }
            public bool Purchased { get; set; }
        }
        public class UpdateVinylDetails: VinylDetails
        {
        }
    }
}
