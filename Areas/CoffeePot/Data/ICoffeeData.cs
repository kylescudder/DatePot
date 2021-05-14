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
		Task<List<RandomCoffee>> GetRandomCoffee(int? UserGroupID);
		Task<int> UpdateCoffee(int CoffeeID, string CoffeeName, int KyleTaste, int RhiannTaste, int KyleExperience, int RhiannExperience);
	}
}
