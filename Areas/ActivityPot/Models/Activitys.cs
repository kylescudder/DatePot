using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace DatePot.Areas.ActivityPot.Models
{
    public class Activitys
    {
        public class ActivityIndex
        {
            public List<ActivityList> ActivityList { get; set; }
        }
        public class ActivityList
        {
            [Key]
            public int ActivityID { get; set; }
            [Display(Name = "Name")]
            public string ActivityName { get; set; }
            public string Location { get; set; }
            public string Description { get; set; }
            [Display(Name = "Expense")]
            public string ExpenseText { get; set; }
            public int ExpenseID { get; set; }
            public string Prebook { get; set; }
            [Display(Name = "Type")]
            public string ActivityType { get; set; }
            public List<RandomActivity> RandomActivity { get; set; }
        }
        public class NewActivity
        {
            [Required]
            [MaxLength(100, ErrorMessage = "Names must not exceed 100 characters")]
            [MinLength(2, ErrorMessage = "Names must be at least 2 characters long")]
            [DisplayName("Name")]
            public string ActivityName { get; set; }
            [Required]
            public string Location { get; set; }
            public string Description { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "Please select an expense from the list")]
            [DisplayName("Expense")]
            public int ExpenseID { get; set; }
            public int ActivtyTypeID { get; set; }
            public bool Prebook { get; set; }
            public List<ActivityTypes> ActivityTypes { get; set; }
        }
        public class ActivityTypes
        {
            public int ActivityTypeID { get; set; }
            public string ActivityType { get; set; }
        }
        public class ActivityDetails
        {
            [Key]
            public int ActivityID { get; set; }
            [Display(Name = "Name")]
            public string ActivityName { get; set; }
            public string Location { get; set; }
            public string Description { get; set; }
            [Display(Name = "Expense")]
            public int ExpenseID { get; set; }
            [Display(Name = "Activity Type")]
            public int ActivityTypeID { get; set; }
            public bool Prebook { get; set; }
            public int UserGroupID { get; set; }
        }
        public class UpdateActivityDetails: ActivityDetails
        {
        }
        public class RandomActivity : ActivityDetails { }
        public class NewActivityType
        {
            [Required]
            [DisplayName("Activity Type")]
            public string ActivityType { get; set; }
        }
        public class Expenses
        {
            public int ExpenseID { get; set; }
            public string ExpenseText { get; set; }
        }
        public class ActivityxTypes
        {
            public int ActivityXTypeID { get; set; }
            public string ActivityTypeText { get; set; }
        }
    }
}
