using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatePot.Areas.Identity.Data
{
    public interface IIdentityData
    {
        Task<int> AddUser(string UserName, string Id);
        Task<List<Models.Identity.UserList>> GetUser(string Id);
    }
}