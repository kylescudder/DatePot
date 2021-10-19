using System.Collections.Generic;
using System.Threading.Tasks;
using static DatePot.Areas.BeerPot.Models.Beers;
using System.Data;
using DatePot.Db;
using Dapper;
using static DatePot.Models.Site;
using System;

namespace DatePot.Areas.BeerPot.Data
{
    public class BeerData : IBeerData
    {
        private readonly ISqlDb _dataAccess;
        public BeerData(ISqlDb dataAccess
            )
        {
            _dataAccess = dataAccess;
        }

        public async Task<List<BeerDetails>> GetBeerDetails(int BeerID)
        {
            var recs = await _dataAccess.LoadData<BeerDetails, dynamic>(
                "scud97_kssu.spGetBeerDetails",
                new { BeerID },
                "Default");
            return recs;
        }
        public Task<int> UpdateBeer(int BeerID, string BeerName, string Brewery)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("BeerID", BeerID);
            p.Add("BeerName", BeerName);
            p.Add("Brewery", Brewery);

            return _dataAccess.SaveData(
                "scud97_kssu.spUpdateBeer",
                p,
                "Default");
        }
        public Task<int> ArchiveBeer(int BeerID)
        {
            return _dataAccess.SaveData(
                "scud97_kssu.spArchiveBeer",
                new { BeerID },
                "Default");
        }
        public async Task<List<BeerList>> GetBeerList(int? UserGroupID)
        {

            var recs = await _dataAccess.LoadData<BeerList, dynamic>(
                "scud97_kssu.spGetBeerList",
                new { UserGroupID },
                "Default");
            return recs;
        }
        public async Task<int> AddBeer(string BeerName, string Brewery, int? UserGroupID)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("BeerName", BeerName);
            p.Add("Brewery", Brewery);
            p.Add("UserGroupID", UserGroupID);
            p.Add("BeerID", DbType.Int32, direction: ParameterDirection.Output);

            await _dataAccess.SaveData("scud97_kssu.spAddBeer", p, "Default");

            return p.Get<int>("BeerID");
        }
        public async Task<List<RandomBeer>> GetRandomBeer(int? UserGroupID)
        {
            var recs = await _dataAccess.LoadData<RandomBeer, dynamic>(
                "scud97_kssu.spRandomBeer",
                new { UserGroupID },
                "Default");
            return recs;
        }

        public async Task<List<BeerRatings>> GetBeerRatings(int? BeerID)
        {
            var recs = await _dataAccess.LoadData<BeerRatings, dynamic>(
                "scud97_kssu.spGetBeerRatings",
                new { BeerID },
                "Default");
            return recs;
        }
        public async Task<int> UpdateBeerRating(string UserID, int Wankyness, int Taste, int BeerRatingID, int BeerID)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("UserID", UserID);
            p.Add("Wankyness", Wankyness);
            p.Add("Taste", Taste);
            p.Add("BeerRatingID", BeerRatingID);
            p.Add("BeerID", BeerID);

            return await _dataAccess.SaveData("scud97_kssu.spUpdateBeerRating", p, "Default");
        }
    }
}
