using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using MySql.Data.MySqlClient;
using static DatePot.Areas.Identity.Models.Identity;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using DatePot.Db;
using Dapper;

namespace DatePot.Areas.Identity.Data
{
    public class IdentityData : IIdentityData
    {
        private readonly ISqlDb _dataAccess;
        public IdentityData(ISqlDb dataAccess
            //, IHttpContextAccessor httpContextAccessor
            )
        {
            _dataAccess = dataAccess;
            //_httpContextAccessor = httpContextAccessor;
        }
        public Task<int> AddUser(string UserName, string Id)
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spAddUserName"; 
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@UserName", UserName);
            //    cmd.Parameters.AddWithValue("@Id", Id);
            //    cmd.ExecuteNonQuery();
            //    con.Close();
            //}
            DynamicParameters p = new DynamicParameters();
            p.Add("UserName", UserName);
            p.Add("Id", Id);

            return _dataAccess.SaveData(
                "scud97_kssu.spAddUserName",
                p,
                "Default");
        }
        public async Task<List<UserList>> GetUser(string Id)
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spGetUser"; 
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@UserID", Id);
            //    IList<UserList> wsl = new List<UserList>();
            //    var reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        var ws = new UserList();
            //        ws.UserName = reader.GetString("UserName");
            //        wsl.Add(ws);
            //    }
            //    reader.Close();
            //    return (List<UserList>)wsl;
            //}
            var recs = await _dataAccess.LoadData<UserList, dynamic>(
                "scud97_kssu.spGetUser",
                new { Id },
                "Default");
            return recs;
        }
    }
}
