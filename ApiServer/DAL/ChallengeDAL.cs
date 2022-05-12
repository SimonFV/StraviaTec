using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ApiServer.DTOs.Requests;
using ApiServer.DTOs.Responses;

namespace ApiServer.DAL{
    public static class ChallengeDAL{
        public static List<ChallengeResponseDto> GetChallenges(){
            List<ChallengeResponseDto> challenges = new();
            try {
                using (SqlConnection con = new SqlConnection(GetConnection())){
                    string query = @"SELECT * FROM CHALLENGE;";
                    using (SqlCommand cmd = new SqlCommand(query, con)){
                        con.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader()){
                            while (sdr.Read()){
                                ChallengeResponseDto challenge = new(){
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
            catch (Exception err){
                Console.Write(err);
                return null;
            }
            return challenges;
        }
        public static string RegisterChallengeDB(ChallengeRegisterDto challenge)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnection()))
                {
                    string procedure = @"RegisterChallenge";
                    using (SqlCommand cmd = new SqlCommand(procedure, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", 0);
                        cmd.Parameters.AddWithValue("@User", challenge.User);
                        cmd.Parameters.AddWithValue("@Name", challenge.Name);
                        cmd.Parameters.AddWithValue("@Class", challenge.Class);
                        cmd.Parameters.AddWithValue("@Privacy", challenge.Privacy);
                        if(challenge.Privacy){
                            cmd.Parameters.AddWithValue("@GroupName", challenge.GroupName);
                        }else{
                            cmd.Parameters.AddWithValue("@GroupName", "");
                        }
                        cmd.Parameters.AddWithValue("@StartDate", challenge.StartDate.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@EndDate", challenge.EndDate.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@Activity_Type", challenge.Activity_Type);

                        con.Open();
                        int sdr = (int)cmd.ExecuteScalar();
                        con.Close();
                        if (sdr == -1){
                            return "Already Exists";
                        }
                        if(sdr==-2){
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



        private static string GetConnection(){
            return "Server=.;Database=StraviaTecDB;User Id=tec;Password=Stravia.12345;";
        }
    }
}