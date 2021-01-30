using DatePot.Areas.CoffeePot.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatePot.Areas.CoffeePot.Data
{
    public interface ICoffeeData
    {
        Task<int> AddCoffee(string CoffeeName);
        Task<int> ArchiveCoffee(int CoffeeID);
        Task<List<Coffees.CoffeeDetails>> GetCoffeeDetails(int CoffeeID);
        Task<List<Coffees.CoffeeList>> GetCoffeeList();
        Task<List<Coffees.RandomCoffee>> GetRandomCoffee();
        Task<int> UpdateCoffee(int CoffeeID, string CoffeeName, int KyleTaste, int RhiannTaste, int KyleExperience, int RhiannExperience);
    }
}