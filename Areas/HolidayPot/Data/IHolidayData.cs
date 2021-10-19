using System.Collections.Generic;
using System.Threading.Tasks;
using static DatePot.Areas.HolidayPot.Models.Holidays;
using System;

namespace DatePot.Areas.HolidayPot.Data
{
    public interface IHolidayData
    {
        Task<int> AddHoliday(string Country, string City, bool Been, DateTime? DateBeen, int? UserGroupID);
        Task<int> ArchiveHoliday(int HolidayID);
        Task<List<HolidayDetails>> GetHolidayDetails(int HolidayID);
        Task<List<HolidayList>> GetHolidayList(int? UserGroupID);
        Task<int> UpdateHoliday(int HolidayID, string Country, string City, bool Been, DateTime? DateBeen);
    }
}
