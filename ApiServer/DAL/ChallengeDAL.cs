using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ApiServer.DTOs.Requests;
using ApiServer.DTOs.Responses;

namespace ApiServer.DAL
{
    public static class ChallengeDAL
    {
        public static List<ChallengeResponseDto> GetChallenges()
        {
            List<ChallengeResponseDto> challenges = new();
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnection()))
                {
                    string query = @"SELECT * FROM CHALLENGE;";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                ChallengeResponseDto challenge = new()
                                {
                                    Id = (int)sdr["Id"],
                                    UserAdmin = (string)sdr["UserAdmin"],
                                    Class = (string)sdr["Class"],
                                    Privacy = (Boolean)sdr["Privacy"],
                                    StartDate = (DateTime)sdr["StartDate"],
                                    EndDate = (DateTime)sdr["EndDate"],
                                    Activity_Type = (string)sdr["Activity_Type"]
                                };
                                challenges.Add(challenge);
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
            return challenges;
        }
        private static string GetConnection()
        {
            return "Server=.;Database=StraviaTecDB;User Id=tec;Password=Stravia.12345;";
        }
    }
}