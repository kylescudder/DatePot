using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static DatePot.Models.Site;

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
        public class UserAccessToGroup : PotAccess 
        { 
            public string UserID { get; set; }
            public string PotName { get; set; }
            public bool AccessGranted { get; set; }
        }
    }
}
