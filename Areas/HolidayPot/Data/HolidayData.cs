using System.Collections.Generic;
using System.Threading.Tasks;
using static DatePot.Areas.HolidayPot.Models.Holidays;
using System.Data;
using DatePot.Db;
using Dapper;
using static DatePot.Models.Site;
using System;

namespace DatePot.Areas.HolidayPot.Data
{
    public class HolidayData : IHolidayData
    {
        private readonly ISqlDb _dataAccess;
        public HolidayData(ISqlDb dataAccess
            )
        {
            _dataAccess = dataAccess;
        }

        public async Task<List<HolidayDetails>> GetHolidayDetails(int HolidayID)
        {
            var recs = await _dataAccess.LoadData<HolidayDetails, dynamic>(
                "scud97_kssu.spGetHolidayDetails",
                new { HolidayID },
                "Default");
            return recs;
        }
        public Task<int> UpdateHoliday(int HolidayID, string Country, string City, bool Been, DateTime? DateBeen)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("HolidayID", HolidayID);
            p.Add("Country", Country);
            p.Add("City", City);
            p.Add("Been", Been);
            p.Add("DateBeen", DateBeen);

            return _dataAccess.SaveData(
                "scud97_kssu.spUpdateHoliday",
                p,
                "Default");
        }
        public Task<int> ArchiveHoliday(int HolidayID)
        {
            return _dataAccess.SaveData(
                "scud97_kssu.spArchiveHoliday",
                new { HolidayID },
                "Default");
        }
        public async Task<List<HolidayList>> GetHolidayList(int? UserGroupID)
        {

            var recs = await _dataAccess.LoadData<HolidayList, dynamic>(
                "scud97_kssu.spGetHolidayList",
                new { UserGroupID },
                "Default");
            return recs;
        }
        public async Task<int> AddHoliday(string Country, string City, bool Been, DateTime? DateBeen, int? UserGroupID)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Country", Country);
            p.Add("City", City);
            p.Add("Been", Been);
            p.Add("DateBeen", DateBeen);
            p.Add("UserGroupID", UserGroupID);
            p.Add("HolidayID", DbType.Int32, direction: ParameterDirection.Output);

            await _dataAccess.SaveData("scud97_kssu.spAddHoliday", p, "Default");

            return p.Get<int>("HolidayID");
        }
    }
}
