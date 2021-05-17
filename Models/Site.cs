using System.ComponentModel.DataAnnotations;

namespace DatePot.Models
{
    public class Site
    {
        public class UserList
        {
            [Key]
            public string UserID { get; set; }
            [Display(Name = "Name")]
            public string UserName { get; set; }
        }
        public class PotAccess
        {
            public int PotID { get; set; }
        }
    }
}