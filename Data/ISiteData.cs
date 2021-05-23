using System.Collections.Generic;
using System.Threading.Tasks;
using DatePot.Areas.Identity.Models;
using DatePot.Models;
using static DatePot.Models.Site;

namespace DatePot.Data
{
    public interface ISiteData
    {
        Task<int> AddUserGroup(string UserID);
        Task<int> GetDefaultUserGroupID(string UserID);
        Task<List<PotAccess>> GetPotAccess(string UserID, int? UserGroupID);
        Task<int> GetPotCount();
        Task<List<UserAccess>> GetUserAccessToGroup(string UserID, int UserGroupID);
        Task<List<UserGroups>> GetUserGroups(string UserID);
        Task<int> GetUserOwnGroup(string UserID);
        Task<List<Identity.UserAccessToGroup>> GetUserPotAccess(string UserID, int? UserGroupID);
        Task<int> SetDefaultUserGroupID(int UserGroupID, string UserID);
        Task<int> UpdateUserAccessToGroup(List<Site.UserAccessToGroup> uatg, string UserID, int UserGroupID);
    }
}