using System.Collections.Generic;
using System.Threading.Tasks;
using DatePot.Areas.Identity.Models;
using DatePot.Models;
using static DatePot.Models.Site;

namespace DatePot.Data
{
    public interface ISiteData
    {
        Task<int> AcceptInvite(string UserID, int UserGroupID);
        Task<int> AddInviteLink(string InviteLink, bool IsConsumed);
        Task<int> AddUserGroup(string UserID);
        Task<int> ConsumeLink(string InviteLink);
        Task<int> GetDefaultUserGroupID(string UserID);
        Task<int> GetExistingPotAccess(string UserID, int UserGroupID);
        Task<List<PotAccess>> GetPotAccess(string UserID, int? UserGroupID);
        Task<int> GetPotCount();
        Task<List<RejectAudit>> GetRejectAudit(string UserID, string ChosenUserID);
        Task<List<UserAccess>> GetUserAccessToGroup(string UserID, int UserGroupID);
        Task<List<UserGroups>> GetUserGroups(string UserID);
        Task<List<Site.UserList>> GetUserList(int? UserGroupID, int PotID);
        Task<int> GetUserOwnGroup(string UserID);
        Task<List<Identity.UserAccessToGroup>> GetUserPotAccess(string UserID, int? UserGroupID);
        Task<bool> HasLinkExpired(string InviteLink);
        Task<bool> IsLinkConsumed(string InviteLink);
        Task<int> RejectInvite(string UserID, int UserGroupID, string SentByID);
        Task<int> SetDefaultUserGroupID(int UserGroupID, string UserID);
        Task<int> UpdateUserAccessToGroup(List<Site.UserAccessToGroup> uatg, string UserID, int UserGroupID);
    }
}