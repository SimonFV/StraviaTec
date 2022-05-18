using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ApiServer.DTOs.Requests;
using ApiServer.DTOs.Responses;

namespace ApiServer.DAL
{
    public static class RaceDAL
    {
        public static List<RaceResponseDto> GetRaces()
        {
            List<RaceResponseDto> races = new();
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnection()))
                {
                    string query = @"SELECT * FROM RACE;";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                RaceResponseDto race = new()
                                {
                                    Id = (int)sdr["Id"],
                                    UserAdmin = (string)sdr["UserAdmin"],
                                    Name = (string)sdr["Name"],
                                    Route = (string)sdr["Route"],
                                    Cost = (float)sdr["Cost"],
                                    Privacy = (Boolean)sdr["Privacy"],
                                    StartDate = (DateTime)sdr["StartDate"],
                                    Category = (string)sdr["Category"],
                                    Type = (string)sdr["Type"]
                                };
                                races.Add(race);
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
            return races;
        }
        public static string RegisterRaceDB(RaceRegisterDto race)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnection()))
                {
                    string procedure = @"RegisterRase";
                    using (SqlCommand cmd = new SqlCommand(procedure, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", 0);
                        cmd.Parameters.AddWithValue("@Admin", race.UserAdmin);
                        cmd.Parameters.AddWithValue("@Name", race.Name);
                        cmd.Parameters.AddWithValue("@Route", race.Route);
                        cmd.Parameters.AddWithValue("@Cost", race.Cost);
                        cmd.Parameters.AddWithValue("@Privacy", race.Privacy);
                        if (race.Privacy)
                        {
                            cmd.Parameters.AddWithValue("@Groups", race.Groups);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Groups", "");
                        }
                        cmd.Parameters.AddWithValue("@StartDate", race.StartDate.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@Category", race.Category);
                        cmd.Parameters.AddWithValue("@Type", race.Type);

                        con.Open();
                        int sdr = (int)cmd.ExecuteScalar();
                        con.Close();
                        if (sdr == -1)
                        {
                            return "Already Exists";
                        }
                        if (sdr == -2)
                        {
                            return "Activity Type not found";
                        }
                    }
                }
            }
            catch (Exception err)
            {
                Console.Write(err);
                return "Error";
            }
            return "Done";
        }



        private static string GetConnection()
        {
            return "Server=.;Database=StraviaTecDB;User Id=tec;Password=Stravia.12345;";
        }
    }
}