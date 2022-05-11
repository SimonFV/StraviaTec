using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ApiServer.DTOs.Responses;

namespace ApiServer.DAL
{
    public static class UserDAL
    {
        public static List<UserResponseDto> GetUsers()
        {
            List<UserResponseDto> users = new();
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnection()))
                {
                    string query = @"SELECT * FROM " + "\"User\"" + ";";
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        con.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                UserResponseDto user = new()
                                {
                                    User = (string)sdr["User"],
                                    FirstName = (string)sdr["FirstName"],
                                    LastName1 = (string)sdr["LastName1"],
                                    LastName2 = (string)sdr["LastName2"],
                                    BirthDate = (DateTime)sdr["BirthDate"],
                                    Picture = (string)sdr["Picture"],
                                    Nationality = (string)sdr["Nationality"]
                                };
                                users.Add(user);
                            }
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception err)
            {
                Console.Write(err);
                return null;
            }
            return users;
        }

        private static string GetConnection()
        {
            return "Server=.;Database=StraviaTecDB;User Id=tec;Password=Stravia.12345;";
        }
    }
}