using System.Collections.Generic;
using System.Threading.Tasks;
using static DatePot.Models.Site;

namespace DatePot.Data
{
	public interface ISiteData
	{
		Task<int> GetDefaultUserGroupID(string UserID);
		Task<List<PotAccess>> GetPotAccess(string UserID);
		Task<List<UserGroups>> GetUserGroups(string UserID);
		Task<int> GetUserOwnGroup(string UserID);
		Task<int> SetDefaultUserGroupID(int UserGroupID, string UserID);
	}
}