using System.ComponentModel;
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
        public class UserGroups
        {
            public int UserGroupID { get; set; }
            public string Name { get; set; }
        }
        public class UserAccess
        {
            public string UserName { get; set; }
        }
        public class UserAccessToGroup : PotAccess
        {
            public string UserID { get; set; }
            public int UserGroupID { get; set; }
        }
        public class NewUserAccess
        {
            [Required]
            [MaxLength(100, ErrorMessage = "Names must not exceed 100 characters")]
            [MinLength(2, ErrorMessage = "Names must be at least 2 characters long")]
            [DisplayName("Email")]
            public string UserEmail { get; set; }
        }
    }
}