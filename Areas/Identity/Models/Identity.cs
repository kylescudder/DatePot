using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace DatePot.Areas.Identity.Models
{
    public class Identity
    {
        public class UserList
        {
            [Key]
            public int UserID { get; set; }
            [Display(Name = "Name")]
            public string UserName { get; set; }
        }
    }
}
