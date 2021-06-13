using System.Collections.Generic;
using System.Threading.Tasks;
using static DatePot.Areas.CoffeePot.Models.Coffees;

namespace DatePot.Areas.CoffeePot.Data
{
    public interface ICoffeeData
    {
        Task<int> AddCoffee(string CoffeeName, int? UserGroupID);
        Task<int> ArchiveCoffee(int CoffeeID);
        Task<List<CoffeeDetails>> GetCoffeeDetails(int CoffeeID);
        Task<List<CoffeeList>> GetCoffeeList(int? UserGroupID);
        Task<List<CoffeeRatings>> GetCoffeeRatings(int? CoffeeID);
        Task<List<RandomCoffee>> GetRandomCoffee(int? UserGroupID);
        Task<int> UpdateCoffee(int CoffeeID, string CoffeeName);
        Task<int> UpdateCoffeeRating(string UserID, int? Experience, int? Taste, int CoffeeRatingID, int CoffeeID);
    }
}
