using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ApiServer.DTOs.Requests;
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
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
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

        public static List<FriendsFrontPage> GetFriendsFrontPageDB()
        {
            List<FriendsFrontPage> friends = new();
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnection()))
                {
                    string query = @"SELECT * FROM MyFriendsStartPage;";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                FriendsFrontPage friend = new()
                                {
                                    User = (string)sdr["User"],
                                    FirstName = (string)sdr["FirstName"],
                                    LastName1 = (string)sdr["LastName1"],
                                    LastName2 = (string)sdr["LastName2"],
                                    Type = (string)sdr["Type"],
                                    Start = (DateTime)sdr["Start"],
                                    Route = (string)sdr["Route"],
                                    Distance = (float)sdr["Distance"]
                                };
                                friends.Add(friend);
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
            return friends;
        }


        public static string RegisterUserDB(UserRegisterDto user)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnection()))
                {
                    string procedure = @"Register";
                    using (SqlCommand cmd = new SqlCommand(procedure, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@User", user.User);
                        cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                        cmd.Parameters.AddWithValue("@LastName1", user.LastName1);
                        cmd.Parameters.AddWithValue("@LastName2", user.LastName2);
                        cmd.Parameters.AddWithValue("@BirthDate", user.BirthDate.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@Password", user.Password);
                        cmd.Parameters.AddWithValue("@Picture", user.Picture);
                        cmd.Parameters.AddWithValue("@Nationality", user.Nationality);

                        con.Open();
                        int sdr = (int)cmd.ExecuteScalar();
                        con.Close();
                        if (sdr == -1)
                            return "Taken";
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

        public static string LoginUserDB(UserLoginDto user)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnection()))
                {
                    string procedure = @"LoginUser";
                    using (SqlCommand cmd = new SqlCommand(procedure, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@User", user.User);
                        cmd.Parameters.AddWithValue("@Password", user.Password);

                        con.Open();
                        int sdr = (int)cmd.ExecuteScalar();
                        con.Close();
                        if (sdr == -1)
                            return "NotFound";
                        else if (sdr == -2)
                            return "WrongPass";
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

        public static string AddActivity(ActivityDto act)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnection()))
                {
                    string procedure = @"AddActivity";
                    using (SqlCommand cmd = new SqlCommand(procedure, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserId", act.UserId);
                        cmd.Parameters.AddWithValue("@Distance", act.Distance);
                        cmd.Parameters.AddWithValue("@Duration", act.Duration.ToString("HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@Route", act.Route);
                        cmd.Parameters.AddWithValue("@Altitude", act.Altitude);
                        cmd.Parameters.AddWithValue("@Start", act.Start.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@Type", act.Type);

                        con.Open();
                        int sdr = (int)cmd.ExecuteScalar();
                        con.Close();
                        if (sdr == -1)
                            return "Taken";
                        else if (sdr == -2)
                            return "CurrentDate";
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