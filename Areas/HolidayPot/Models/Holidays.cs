using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DatePot.Areas.HolidayPot.Models
{
    public class Holidays
    {
        public class HolidayIndex
        {
            public List<HolidayList> HolidayList { get; set; }
        }
        public class HolidayList
        {
            [Key]
            public int HolidayID { get; set; }
            [Display(Name = "Name")]
            public string Country { get; set; }
            public string City { get; set; }
            public string Been { get; set; }
            public string DateBeen { get; set; }
            public int? UserGroupID { get; set; }
        }
        public class NewHoliday
        {
            [Required]
            public string Country { get; set; }
            [Required]
            public string City { get; set; }
            [Required]
            public bool Been { get; set; }
            public DateTime? DateBeen { get; set; }
        }
        public class HolidayDetails
        {
            [Key]
            public int HolidayID { get; set; }
            [Display(Name = "Name")]
            public string Country { get; set; }
            public string City { get; set; }
            public bool Been { get; set; }
            public DateTime DateBeen { get; set; }
            public int? UserGroupID { get; set; }
        }
        public class UpdateHolidayDetails : HolidayDetails
        {
            public IFormFile FormFile { get; set; }
        }
        public class FileModel
        {
            public string FileName { get; set; }
        }
    }
}
