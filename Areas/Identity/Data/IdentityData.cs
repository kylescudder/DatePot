using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using static DatePot.Areas.Identity.Models.Identity;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DatePot.Areas.Identity.Data
{
    public class IdentityData
    {
        public void AddUser(string cs, string UserName, string Id)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "spAddUserName"; 
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@UserName", UserName);
                cmd.Parameters.AddWithValue("@Id", Id);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public List<UserList> GetUser(string cs, string Id)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "spGetUser"; 
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@UserID", Id);
                IList<UserList> wsl = new List<UserList>();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var ws = new UserList();
                    ws.UserName = reader.GetString("UserName");
                    wsl.Add(ws);
                }
                reader.Close();
                return (List<UserList>)wsl;
            }
        }
    }
}
