using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ApiServer.DTOs.Requests;
using ApiServer.DTOs.Responses;

namespace ApiServer.DAL
{
    /// <summary>
    /// DAL for Challenge related SQL requests. 
    /// </summary>
    public static class ChallengeDAL
    {
        /// <summary>
        /// Get all the challenges from database
        /// </summary>
        /// <returns><c>List</c> with the Challenges or an error message</returns>
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
                                    Name=(string)sdr["Name"],
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

        /// <summary>
        /// Register Challenge participation in the database
        /// </summary>
        /// <param name="user"><c>string</c>: User account</param>
        /// <param name="challengeId"><c>int</c>: Challenge Id to participate on</param>
        /// <returns><c>string</c> with the participation result</returns>

        public static string GetInChallenge(string user, int challengeId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnection()))
                {
                    string query = @"INSERT INTO CHALLENGE_PARTICIPANTS (" + "\"User\"" + ", ChallengeId) " +
                                    "VALUES('" + user + "', " + challengeId + ");";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        SqlDataReader sdr = cmd.ExecuteReader();
                        con.Close();
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

        /// <summary>
        /// Get the visibility of all the challenges from the database
        /// </summary>
        /// <returns><c>List</c> Challenges and their visibility or an error message</returns>
        public static List<ChallengeVisibilityDto> GetChallengeVisibility()
        {
            List<ChallengeVisibilityDto> challenges = new();
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnection()))
                {
                    string query = @"SELECT * FROM CHALLENGE_VISIBILITY ;";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                ChallengeVisibilityDto challenge = new()
                                {
                                    GroupId = (int)sdr["GroupId"],
                                    ChallengeId = (int)sdr["ChallengeId"]
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

        /// <summary>
        /// Register a new Challenge in the database
        /// </summary>
        /// <param name="challenge"><c>string</c>: New Challenge name</param>
        /// <returns><c>string</c> With the registration result</returns>
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
                        cmd.Parameters.AddWithValue("@Objective", challenge.Objective);
                        if (challenge.Privacy)
                        {
                            cmd.Parameters.AddWithValue("@Groups", challenge.Groups);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Groups", "");
                        }
                        cmd.Parameters.AddWithValue("@StartDate", challenge.StartDate.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@EndDate", challenge.EndDate.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@Activity_Type", challenge.Activity_Type);

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

        /// <summary>
        /// Gets all the challenges from the database to which the user is subscribed
        /// </summary>
        /// <param name="user"><c>string</c>: User account</param>
        /// <returns><c>List</c>: Challenges to which the user is subscribed</returns>
        public static List<ChallengeResponseDto> GetChallengesByUser(string user)
        {
            List<ChallengeResponseDto> challenges = new();
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnection()))
                {
                    string query = 
                        @"SELECT *" +
                        "FROM CHALLENGE WHERE CHALLENGE.Id IN(SELECT ChallengeId FROM CHALLENGE_PARTICIPANTS WHERE "
                        + "\"User\"" +"='"+user+"');";
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
                                    UserAdmin=(string)sdr["UserAdmin"],
                                    Name = (string)sdr["Name"],
                                    Class=(string)sdr["Class"],
                                    Privacy=(Boolean)sdr["Privacy"],
                                    StartDate=(DateTime)sdr["StartDate"],
                                    EndDate=(DateTime)sdr["EndDate"],
                                    Activity_Type=(string)sdr["Activity_Type"],
                                    Objective=(Decimal)sdr["Objective"]


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

        /// <summary>
        /// Add an Activity to a Challenge in the database
        /// </summary>
        /// <param name="challengeId"><c>int</c>: Challenge Id</param>
        /// <param name="activityId"><c>int</c>: Activity Id</param>
        /// <returns><c>string</c> with the binding result</returns>
        public static string AddChallengeActivity(int challengeId, int activityId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnection()))
                {
                    string query = @"INSERT INTO CHALLENGE_ACTIVITIES(ChallengeId, ActivityId) VALUES (" + 
                    challengeId +"," + activityId+");";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        SqlDataReader sdr = cmd.ExecuteReader();
                        con.Close();
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


        public static Decimal GetChallengProgres(int challengeId, string user)
        {
            Decimal progress = new();
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnection()))
                {
                    string query =
                        @"SELECT SUM(Distance) AS Progress FROM ACTIVITY WHERE Id IN(
                            SELECT ActivityId FROM CHALLENGE_ACTIVITIES WHERE ChallengeId="+challengeId+") AND UserId='"+
                            user+"';";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                progress = (Decimal)sdr["Progress"];
                            }
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception err)
            {
                Console.Write(err);
                return -1;
            }
            return progress;
        }



        private static string GetConnection()
        {
            return "Server=.;Database=StraviaTecDB;User Id=tec;Password=Stravia.12345;";
        }
    }
}