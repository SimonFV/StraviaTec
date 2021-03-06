using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ApiServer.DTOs;
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

        public static UserResponseDto GetUser(string userId)
        {
            UserResponseDto user = new() { User = "" };
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnection()))
                {
                    string query = @"SELECT * FROM " + "\"USER\"" +
                                    " WHERE " + "\"User\" = '" + userId + "';";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                user.User = (string)sdr["User"];
                                user.FirstName = (string)sdr["FirstName"];
                                user.LastName1 = (string)sdr["LastName1"];
                                user.LastName2 = (string)sdr["LastName2"];
                                user.BirthDate = (DateTime)sdr["BirthDate"];
                                user.Picture = (string)sdr["Picture"];
                                user.Nationality = (string)sdr["Nationality"];
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
            return user;
        }

        public static string UpdateUser(UserRegisterDto user, string currenPassword)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnection()))
                {
                    string procedure = @"UpdateUser";
                    using (SqlCommand cmd = new SqlCommand(procedure, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@User", user.User);
                        cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                        cmd.Parameters.AddWithValue("@LastName1", user.LastName1);
                        cmd.Parameters.AddWithValue("@LastName2", user.LastName2);
                        cmd.Parameters.AddWithValue("@BirthDate", user.BirthDate.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@Password", currenPassword);
                        cmd.Parameters.AddWithValue("@NewPassword", user.Password);
                        cmd.Parameters.AddWithValue("@Picture", user.Picture);
                        cmd.Parameters.AddWithValue("@Nationality", user.Nationality);

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
                return "error";
            }
            return "done";
        }

        public static List<FriendsFrontPage> GetFriendsFrontPageDB(string user)
        {
            List<FriendsFrontPage> friends = new();
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnection()))
                {
                    string procedure = @"FriendsLatestActivities";
                    using (SqlCommand cmd = new SqlCommand(procedure, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@User", user);
                        con.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                FriendsFrontPage friend = new()
                                {
                                    Id = (int)sdr["Id"],
                                    User = (string)sdr["User"],
                                    FirstName = (string)sdr["FirstName"],
                                    LastName1 = (string)sdr["LastName1"],
                                    LastName2 = (string)sdr["LastName2"],
                                    Type = (string)sdr["Type"],
                                    Start = (DateTime)sdr["Start"],
                                    Route = (string)sdr["Route"],
                                    Distance = (decimal)sdr["Distance"]
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

        public static List<UserResponseDto> GetFriendsAvailable(string myuser)
        {
            List<UserResponseDto> users = new();
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnection()))
                {
                    string procedure = @"FriendsAvailable";
                    using (SqlCommand cmd = new SqlCommand(procedure, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@User", myuser);
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


        public static List<UserResponseDto> GetFriends(string myuser)
        {
            List<UserResponseDto> users = new();
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnection()))
                {
                    string procedure = @"getFriends";
                    using (SqlCommand cmd = new SqlCommand(procedure, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@User", myuser);
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
                                    //LastName2 = (string)sdr["LastName2"],
                                    BirthDate = (DateTime)sdr["BirthDate"],
                                    Picture = (string)sdr["Picture"],
                                    //Nationality = (string)sdr["Nationality"]
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

        public static Boolean DeleteFriend(string user, string friend)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnection()))
                {
                    string query = @"DELETE FROM FRIENDS WHERE " + "\"User\" = '" + user + "' AND FriendUser='" + friend + "';";
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
                return false;
            }
            return true;
        }

        public static List<GroupDTO> GetGroupsAvailable(string user)
        {
            List<GroupDTO> groups = new();
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnection()))
                {
                    string procedure = @"GroupsAvailable";
                    using (SqlCommand cmd = new SqlCommand(procedure, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@User", user);
                        con.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                GroupDTO group = new()
                                {
                                    Id = (int)sdr["Id"],
                                    AdminUser = (string)sdr["AdminUser"],
                                    Name = (string)sdr["Name"]
                                };
                                groups.Add(group);
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
            return groups;
        }


        public static List<GroupDTO> GetGroups(string user)
        {
            List<GroupDTO> groups = new();
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnection()))
                {
                    string procedure = @"getGroups";
                    using (SqlCommand cmd = new SqlCommand(procedure, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@User", user);
                        con.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                GroupDTO group = new()
                                {
                                    Id = (int)sdr["Id"],
                                    AdminUser = (string)sdr["AdminUser"],
                                    Name = (string)sdr["Name"]
                                };
                                groups.Add(group);
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
            return groups;
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
                        cmd.Parameters.AddWithValue("@Start", act.Start.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@Type", act.Type);
                        cmd.Parameters.AddWithValue("@RoC", act.RoC);
                        cmd.Parameters.AddWithValue("@RoCName", act.RoCName);

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


        public static string UpdateActivity(int actId, ActivityDto act)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnection()))
                {
                    string procedure = @"UpdateActivity";
                    using (SqlCommand cmd = new SqlCommand(procedure, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserId", act.UserId);
                        cmd.Parameters.AddWithValue("@ActId", actId);
                        cmd.Parameters.AddWithValue("@Distance", act.Distance);
                        cmd.Parameters.AddWithValue("@Duration", act.Duration.ToString("HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@Route", act.Route);
                        cmd.Parameters.AddWithValue("@Start", act.Start.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@Type", act.Type);
                        cmd.Parameters.AddWithValue("@RoC", act.RoC);
                        cmd.Parameters.AddWithValue("@RoCName", act.RoCName);

                        con.Open();
                        int sdr = (int)cmd.ExecuteScalar();
                        con.Close();
                        if (sdr == -1)
                            return "Activity Not Found";
                        else if (sdr == -2)
                            return "Already in Database";
                        else if (sdr == -3)
                            return "Date doesn't match";
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

        public static int GetActivityId(ActivityDto act)
        {
            int id = new();
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnection()))
                {

                    string query = @"SELECT Id FROM ACTIVITY WHERE UserId='" + act.UserId + "' AND Duration= '"
                    + act.Duration.ToString("HH:mm:ss") + "' AND " + "\"Start\"" + "='" + act.Start.ToString("yyyy-MM-dd HH:mm:ss") + "';";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        SqlDataReader sdr = cmd.ExecuteReader();
                        while (sdr.Read())
                        {

                            id = (int)sdr["Id"];
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
            return id;
        }

        public static void UpdateActivityRoute(ActivityDto act, int actId)
        {
            using (SqlConnection con = new SqlConnection(GetConnection()))
            {
                string query = @"UPDATE ACTIVITY SET Route = '" + act.Route + "' " +
                                "WHERE Id = " + actId + ";";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();
                    con.Close();
                }
            }
        }


        public static List<ActivityResponseDto> GetuserActivities(string name)
        {
            List<ActivityResponseDto> Activities = new();
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnection()))
                {
                    string query = @"SELECT * FROM ACTIVITY WHERE UserId ='" + name + "';";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        SqlDataReader sdr = cmd.ExecuteReader();
                        while (sdr.Read())
                        {
                            ActivityResponseDto activity = new()
                            {
                                ActivityId= (int)sdr["Id"],
                                UserId = (string)sdr["UserId"],
                                Distance = (Decimal)sdr["Distance"],
                                Duration = (TimeSpan)sdr["Duration"],
                                Route = (string)sdr["Route"],
                                Start = (DateTime)sdr["Start"],
                                Type = (string)sdr["Type"]
                            };
                            Activities.Add(activity);
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
            return Activities;
        }

        public static string AddFriend(string user, string friend)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnection()))
                {
                    string query = @"INSERT INTO FRIENDS(" + "\"User\"" + ", FriendUser) " +
                                    "VALUES('" + user + "', '" + friend + "');";
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

        public static string CreateGroup(string admin, string name)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnection()))
                {
                    string query = @"INSERT INTO GROUPS(AdminUser, " + "\"Name\"" + ") " +
                                    "VALUES('" + admin + "', '" + name + "');";
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

        public static string JoinGroup(int id, string user)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnection()))
                {
                    string query = @"INSERT INTO GROUP_USERS(GroupId, " + "\"User\"" + ") " +
                                    "VALUES(" + id + ", '" + user + "');";
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

        public static string QuitGroup(int id, string user)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnection()))
                {
                    string query = @"DELETE FROM GROUP_USERS WHERE" + "\"User\"" + "='" + user +
                                    "'AND GroupId=" + id + ";";
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

        public static string UpdateGroup(GroupDTO group)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnection()))
                {
                    string query = @"UPDATE GROUPS SET AdminUser = '" + group.AdminUser +
                                    "', " + "\"Name\"" + "= '" + group.Name +
                                    "' WHERE Id = " + group.Id + ";";
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



        public static string DeleteUser(string user)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnection()))
                {
                    string query = @"DELETE FROM " + "\"USER\" " +
                                    "WHERE " + "\"User\" = '" + user + "';";
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


        private static string GetConnection()
        {
            return "Server=tcp:straviatec-server.database.windows.net,1433;Initial Catalog=StraviaTecDB-Azure;Persist Security Info=False;User ID=serverAdmin;Password=StraviaTEC-123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        }
    }
}