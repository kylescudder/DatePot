using System.Collections.Generic;
using System.Threading.Tasks;
using static DatePot.Areas.BeerPot.Models.Beers;
using static DatePot.Models.Site;

namespace DatePot.Areas.BeerPot.Data
{
    public interface IBeerData
    {
        Task<int> AddBeer(string BeerName, string Brewery, int? UserGroupID);
        Task<int> ArchiveBeer(int BeerID);
        Task<List<BeerDetails>> GetBeerDetails(int BeerID);
        Task<List<BeerList>> GetBeerList(int? UserGroupID);
        Task<List<BeerRatings>> GetBeerRatings(int? BeerID);
        Task<List<RandomBeer>> GetRandomBeer(int? UserGroupID);
        Task<List<UserList>> GetUserList(int? UserGroupID);
        Task<int> UpdateBeer(int BeerID, string BeerName, string Brewery);
    }
}
