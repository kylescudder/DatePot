using System.ComponentModel.DataAnnotations;

namespace DatePot.Areas.Identity.Models
{
    public class Identity
    {
        public class UserList
        {
            [Key]
            public string UserID { get; set; }
            [Display(Name = "Name")]
            public string Name { get; set; }
            public int DefaultUserGroupID { get; set; }
        }
    }
}
