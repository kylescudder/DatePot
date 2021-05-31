using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using DapperParameters;
using DatePot.Db;
using static DatePot.Areas.Identity.Models.Identity;
using static DatePot.Models.Site;

namespace DatePot.Data
{
    public class SiteData : ISiteData
    {
        private readonly ISqlDb _dataAccess;
        public SiteData(ISqlDb dataAccess
            )
        {
            _dataAccess = dataAccess;
        }
        public async Task<List<PotAccess>> GetPotAccess(string UserID, int? UserGroupID)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("UserID", UserID);
            p.Add("UserGroupID", UserGroupID);

            var recs = await _dataAccess.LoadData<PotAccess, dynamic>(
                "scud97_kssu.spGetPotAccess",
                p,
                "Default");
            return recs;
        }
        public async Task<List<Areas.Identity.Models.Identity.UserAccessToGroup>> GetUserPotAccess(string UserID, int? UserGroupID)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("UserID", UserID);
            p.Add("UserGroupID", UserGroupID);

            var recs = await _dataAccess.LoadData<Areas.Identity.Models.Identity.UserAccessToGroup, dynamic>(
                "scud97_kssu.spGetUserPotAccess",
                p,
                "Default");
            foreach (var item in recs)
            {
                item.UserID = UserID;
            }
            return recs;
        }
        public async Task<int> GetUserOwnGroup(string UserID)
        {
            var recs = await _dataAccess.LoadData<int, dynamic>(
                "scud97_kssu.spGetUserGroupID",
                new { UserID },
                "Default");
            return recs[0];
        }
        public async Task<List<UserGroups>> GetUserGroups(string UserID)
        {
            var recs = await _dataAccess.LoadData<UserGroups, dynamic>(
                "scud97_kssu.spGetUserGroups",
                new { UserID },
                "Default");
            return recs;
        }
        public async Task<int> GetDefaultUserGroupID(string UserID)
        {
            var recs = await _dataAccess.LoadData<int, dynamic>(
                "scud97_kssu.spGetDefaultUserGroupID",
                new { UserID },
                "Default");
            if (recs.Count > 0)
            {
                return recs[0];
            }
            else
            {
                return 0;
            }
        }
        public Task<int> SetDefaultUserGroupID(int UserGroupID, string UserID)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("UserGroupID", UserGroupID);
            p.Add("UserID", UserID);

            return _dataAccess.SaveData(
                "scud97_kssu.spUpdateDefaultUserGroupID",
                p,
                "Default");
        }
        public async Task<List<UserAccess>> GetUserAccessToGroup(string UserID, int UserGroupID)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("UserID", UserID);
            p.Add("UserGroupID", UserGroupID);
            var recs = await _dataAccess.LoadData<UserAccess, dynamic>(
                "scud97_kssu.spGetMyUserGroupMembers",
                p,
                "Default");
            return recs;
        }
        public Task<int> UpdateUserAccessToGroup(List<Models.Site.UserAccessToGroup> uatg, string UserID, int UserGroupID)
        {
            var p = new DynamicParameters();
            p.AddTable("@tblUserGroups", "tblusergroupstype ", uatg);
            p.Add("UserID", UserID);
            p.Add("UserGroupID", UserGroupID);
            return _dataAccess.SaveData(
                "scud97_kssu.spUpdateUserAccessToGroup",
                p,
                "Default");
        }
        public async Task<int> GetPotCount()
        {
            var recs = await _dataAccess.LoadData<int, dynamic>(
                "scud97_kssu.spGetPotCount",
                new { },
                "Default");
            return recs[0];
        }
        public Task<int> AddUserGroup(string UserID)
        {
            return _dataAccess.SaveData(
                "scud97_kssu.spAddUserGroup",
                new { UserID },
                "Default");
        }
        public Task<int> AcceptInvite(string UserID, int UserGroupID)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("UserID", UserID);
            p.Add("UserGroupID", UserGroupID);
            return _dataAccess.SaveData(
                "scud97_kssu.spAcceptInvite",
                p,
                "Default");
        }
        public Task<int> RejectInvite(string UserID, int UserGroupID, string SentByID)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("UserID", UserID);
            p.Add("UserGroupID", UserGroupID);
            p.Add("SentByID", SentByID);
            return _dataAccess.SaveData(
                "scud97_kssu.spRejectInvite",
                p,
                "Default");
        }
        public async Task<int> GetExistingPotAccess(string UserID, int UserGroupID)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("UserID", UserID);
            p.Add("UserGroupID", UserGroupID);

            var recs = await _dataAccess.LoadData<int, dynamic>(
                "scud97_kssu.spGetExistingPotAccess",
                p,
                "Default");
            return recs[0];
        }
        public async Task<List<RejectAudit>> GetRejectAudit(string UserID, string ChosenUserID)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("UserID", UserID);
            p.Add("ChosenUserID", ChosenUserID);
            var recs = await _dataAccess.LoadData<RejectAudit, dynamic>(
                "scud97_kssu.spGetRejectAudit",
                p,
                "Default");
            return recs;
        }
        public Task<int> AddInviteLink(string InviteLink, bool IsConsumed)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("InviteLink", InviteLink);
            p.Add("IsConsumed", IsConsumed);
            return _dataAccess.SaveData(
                "scud97_kssu.spAddInviteLink",
                p,
                "Default");
        }
        public Task<int> ConsumeLink(string InviteLink)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("InviteLink", InviteLink);
            return _dataAccess.SaveData(
                "scud97_kssu.spConsumeLink",
                p,
                "Default");
        }
        public async Task<bool> IsLinkConsumed(string InviteLink)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("InviteLink", InviteLink);
            p.Add("LinkConsumed", DbType.Int32, direction: ParameterDirection.Output);

            await _dataAccess.SaveData("scud97_kssu.spIsLinkConsumed", p, "Default");

            var i = p.Get<int>("LinkConsumed");
            return Convert.ToBoolean(i);
        }
    }
}