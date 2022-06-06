using Microsoft.AspNetCore.Mvc;
using System;
using ApiServer.DAL;
using ApiServer.DTOs.Requests;
using ApiServer.DTOs.Responses;
using ApiServer.DTOs;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace ApiServer.Controllers
{
    /// <summary>
    /// Controller class for user related http requests.
    /// </summary>
    [ApiController]
    
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        public UserController()
        {
        }

        /// <summary>
        /// Gets all the users in the database.
        /// </summary>
        /// <returns><c>IActionResult</c> with the users or an error message</returns>
        [HttpGet]
        [Route("users")]
        public IActionResult GetUsers()
        {
            List<UserResponseDto> users = UserDAL.GetUsers();
            if (users is null)
                return new JsonResult("Something went wrong retrieving the users.") { StatusCode = 500 };
            return Ok(users);
        }

        /// <summary>
        /// Gets the user with the specified <paramref name="userId"/>
        /// </summary>
        /// <param name="userId"><c>string</c>: User account</param>
        /// <returns><c>IActionResult</c> with the user or an error message</returns>
        [HttpGet]
        [Route("users/{userId}")]
        public IActionResult GetUser(string userId)
        {
            UserResponseDto user = UserDAL.GetUser(userId);
            if (user is null)
                return new JsonResult("Something went wrong retrieving the user.") { StatusCode = 500 };
            if (user.User == "")
                return new JsonResult("User not found.") { StatusCode = 400 };
            return Ok(user);
        }

        [HttpGet]
        [Route("friendsFrontPage/{user}")]
        public IActionResult GetFriendsFrontPage(string user)
        {
            List<FriendsFrontPage> friends = UserDAL.GetFriendsFrontPageDB(user);
            if (friends is null)
                return new JsonResult("Something went wrong retrieving the friends.") { StatusCode = 500 };
            return Ok(friends);
        }

        [HttpPost]
        [Route("userImage")]
        public IActionResult GetUserImage(FilePathDto route)
        {
            try
            {
                return File(System.IO.File.ReadAllBytes(route.Path), "image/" + Path.GetExtension(route.Path).Remove(0, 1));
            }
            catch (Exception err)
            {
                Console.Write(err);
                return new JsonResult("Something went wrong retrieving the image.") { StatusCode = 500 };
            }
        }

        [HttpPost]
        [Route("activityRoute")]
        public IActionResult GetActivityRoute(FilePathDto route)
        {
            try
            {
                RouteResponseDto actRoute = new()
                {
                    Center = GpxCenter.getCenter(route.Path),
                    File = File(System.IO.File.ReadAllBytes(route.Path), "text/plain")
                };

                return Ok(actRoute);
            }
            catch (Exception err)
            {
                Console.Write(err);
                return new JsonResult("Something went wrong retrieving the route.") { StatusCode = 500 };
            }
        }

        [HttpGet]
        [Route("friendsAvailable/{user}")]
        public IActionResult GetFriendsAvailable(string user)
        {
            List<UserResponseDto> friends = UserDAL.GetFriendsAvailable(user);
            if (friends is null)
                return new JsonResult("Something went wrong retrieving the users.") { StatusCode = 500 };
            return Ok(friends);
        }

        [HttpGet]
        [Route("friends/{user}")]
        public IActionResult GetFriends(string user)
        {
            List<UserResponseDto> friends = UserDAL.GetFriends(user);
            if (friends is null)
                return new JsonResult("Something went wrong retrieving the users.") { StatusCode = 500 };
            return Ok(friends);
        }

        [HttpDelete]
        [Route("friends/Delete/{user}/{friend}")]
        public IActionResult DeleteFriend(string user, string friend)
        {
            if (UserDAL.DeleteFriend(user, friend))
            {
                return new JsonResult("Friend deleted") { StatusCode = 200 };
            }
            else
            {
                return new JsonResult("Something went wrong deleting friend") { StatusCode = 500 };
            }
        }

        [HttpPost]
        [Route("addActivity2")]
        public IActionResult PostActivity1(ActivityDto activity)
        {
            if (ModelState.IsValid)
            {
                string result = UserDAL.AddActivity(activity);
                if (result is "Error")
                    return new JsonResult("Something went wrong while adding the activity.") { StatusCode = 500 };
                else if (result is "Taken")
                    return new JsonResult("There is another activity registered during that period.") { StatusCode = 409 };
                else if (result is "CurrentDate")
                    return new JsonResult("The period of the activity does not match with the current time") { StatusCode = 409 };
                return new JsonResult("Activity added.") { StatusCode = 201 };
            }
            return new JsonResult("Invalid model for Activity.") { StatusCode = 400 };
        }

        [HttpPost]
        [Route("addActivity")]
        public IActionResult PostActivity(
            [FromForm] string UserId,
            [FromForm] string Distance,
            [FromForm] DateTime Duration,
            [FromForm] IFormFile Route,
            [FromForm] DateTime Start,
            [FromForm] string Type,
            [FromForm] string Roc,
            [FromForm] string RocName)
        {
            try
            {
                var format = new NumberFormatInfo();
                format.NegativeSign = "-";
                format.NumberDecimalSeparator = ".";
                ActivityDto activity = new()
                {
                    UserId = UserId,
                    Distance = Math.Round((decimal)Double.Parse(Distance, format), 3),
                    Duration = Duration,
                    Route = "default",
                    Start = Start,
                    Type = Type,
                    RoC=Roc,
                    RoCName=RocName
                };

                string result = UserDAL.AddActivity(activity);
                if (result is "Error")
                    return new JsonResult("Something went wrong while adding the activity.") { StatusCode = 500 };
                else if (result is "Taken")
                    return new JsonResult("There is another activity registered during that period.") { StatusCode = 409 };
                else if (result is "CurrentDate")
                    return new JsonResult("The period of the activity does not match with the current time") { StatusCode = 409 };

                if (Route is not null)
                {
                    int actId = UserDAL.GetActivityId(activity);
                    string fileName = "";
                    string filePath = "";
                    fileName = Path.GetFileName(Route.FileName);
                    filePath = Path.Combine("Files\\Routes", "Activities", actId.ToString(), fileName);
                    Directory.CreateDirectory("Files\\Routes\\Activities\\" + actId.ToString());
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        Route.CopyToAsync(fileStream);
                    }
                    activity.Route = filePath;
                    UserDAL.UpdateActivityRoute(activity, actId);
                }
                return new JsonResult("Activity added.") { StatusCode = 201 };
            }
            catch (Exception err)
            {
                Console.Write(err);
                return new JsonResult("Something went wrong while updating.") { StatusCode = 500 };
            }
        }


        [HttpPut]
        [Route("UpdateActivity/{actId}")]
        public IActionResult UpdateActivity(int actId, 
            [FromForm] string UserId,
            [FromForm] decimal Distance,
            [FromForm] DateTime Start,
            [FromForm] DateTime Duaration,
            [FromForm] string Route,
            [FromForm] string Type,
            [FromForm] string RoC,
            [FromForm] string RoCName
            )
        {
            try
            {

                ActivityDto act = new()
                {
                    UserId = UserId,
                    Distance = Distance,
                    Duration = Duaration,
                    Route = Route,
                    Start = Start,
                    Type = Type,
                    RoC = RoC,
                    RoCName = RoCName
                };

                string result = UserDAL.UpdateActivity(actId, act);
                if (result is "Error")
                    return new JsonResult("Something went wrong in the registration.") { StatusCode = 500 };
                else if (result is "NotFound")
                    return new JsonResult("User not found.") { StatusCode = 403 };
                else if (result is "WrongPass")
                    return new JsonResult("Incorrect password.") { StatusCode = 403 };
                
                return Ok();
            }
            catch (Exception err)
            {
                Console.Write(err);
                return new JsonResult("Something went wrong while updating.") { StatusCode = 500 };
            }
        }

        [HttpGet]
        [Route("userActivities/{user}")]
        public IActionResult GetuserActivities(string user)
        {
            List<ActivityResponseDto> activities = UserDAL.GetuserActivities(user);
            if (activities is null)
                return new JsonResult("Something went wrong getting the activities.") { StatusCode = 500 };
            return Ok(activities);
        }

        [HttpPost]
        [Route("getActivityId")]
        public IActionResult GetActivityId(ActivityDto activity)
        {
            int Id = UserDAL.GetActivityId(activity);
            if (Id is -1)
                return new JsonResult("Something went wrong getting the activities.") { StatusCode = 500 };
            return Ok(Id);
        }

        [HttpPost]
        [Route("addFriend/{user}/{friend}")]
        public IActionResult PostFriend(string user, string friend)
        {
            if (ModelState.IsValid)
            {
                string result = UserDAL.AddFriend(user, friend);
                if (result is "Error")
                    return new JsonResult("Something went wrong while adding the friend.") { StatusCode = 500 };
                return new JsonResult("Friend added.") { StatusCode = 201 };
            }
            return new JsonResult("Invalid model for User/Friend.") { StatusCode = 400 };
        }

        [HttpGet]
        [Route("groupsAvailable/{user}")]
        public IActionResult GetGroupsAvailable(string user)
        {
            List<GroupDTO> groups = UserDAL.GetGroupsAvailable(user);
            if (groups is null)
                return new JsonResult("Something went wrong retrieving the groups.") { StatusCode = 500 };
            return Ok(groups);
        }

        [HttpGet]
        [Route("groups/{user}")]
        public IActionResult GetGroups(string user)
        {
            List<GroupDTO> groups = UserDAL.GetGroups(user);
            if (groups is null)
                return new JsonResult("Something went wrong retrieving the groups.") { StatusCode = 500 };
            return Ok(groups);
        }

        [HttpPost]
        [Route("createGroup/{admin}/{name}")]
        public IActionResult CreateGroup(string admin, string name)
        {
            if (ModelState.IsValid)
            {
                string result = UserDAL.CreateGroup(admin, name);
                if (result is "Error")
                    return new JsonResult("Something went wrong while creating the group.") { StatusCode = 500 };
                return new JsonResult("Group created.") { StatusCode = 201 };
            }
            return new JsonResult("Invalid model for Admin/Name.") { StatusCode = 400 };
        }

        [HttpPost]
        [Route("joinGroup/{id}/{user}")]
        public IActionResult JoinGroup(int id, string user)
        {
            if (ModelState.IsValid)
            {
                string result = UserDAL.JoinGroup(id, user);
                if (result is "Error")
                    return new JsonResult("Something went wrong while joining the group.") { StatusCode = 500 };
                return new JsonResult("Joined.") { StatusCode = 201 };
            }
            return new JsonResult("Invalid model for Id/User.") { StatusCode = 400 };
        }
        [HttpDelete]
        [Route("quitGroup/{id}/{user}")]
        public IActionResult QuitGroup(int id, string user)
        {
            if (ModelState.IsValid)
            {
                string result = UserDAL.QuitGroup(id, user);
                if (result is "Error")
                    return new JsonResult("Something went wrong while quitting the group.") { StatusCode = 500 };
                return new JsonResult("Done.") { StatusCode = 201 };
            }
            return new JsonResult("Invalid model for Id/User.") { StatusCode = 400 };
        }


        [HttpPut]
        [Route("updateGroup")]
        public IActionResult UpdateGroup(GroupDTO group)
        {
            if (ModelState.IsValid)
            {
                string result = UserDAL.UpdateGroup(group);
                if (result is "Error")
                    return new JsonResult("Something went wrong while updating the group.") { StatusCode = 500 };
                return new JsonResult("Group updated.") { StatusCode = 201 };
            }
            return new JsonResult("Invalid model for Group.") { StatusCode = 400 };
        }


        [HttpPut]
        [Route("edit")]
        public IActionResult UpdateUser(
            [FromForm] string User,
            [FromForm] string FirstName,
            [FromForm] string LastName1,
            [FromForm] string LastName2,
            [FromForm] DateTime BirthDate,
            [FromForm] string NewPassword,
            [FromForm] string CurrentPassword,
            [FromForm] IFormFile Picture,
            [FromForm] string CurrentPicture,
            [FromForm] string Nationality)
        {
            try
            {
                string fileName = "";
                string filePath = "";
                if (Picture is not null)
                {
                    fileName = Path.GetFileName(Picture.FileName);
                    filePath = Path.Combine("Files\\Profiles", User, fileName);
                }
                UserRegisterDto user = new()
                {
                    User = User,
                    FirstName = FirstName,
                    LastName1 = LastName1,
                    LastName2 = LastName2,
                    BirthDate = BirthDate,
                    Password = NewPassword is not null ? NewPassword : CurrentPassword,
                    Picture = Picture is not null ? filePath : CurrentPicture,
                    Nationality = Nationality
                };

                string result = UserDAL.UpdateUser(user, CurrentPassword);
                if (result is "Error")
                    return new JsonResult("Something went wrong in the registration.") { StatusCode = 500 };
                else if (result is "NotFound")
                    return new JsonResult("User not found.") { StatusCode = 403 };
                else if (result is "WrongPass")
                    return new JsonResult("Incorrect password.") { StatusCode = 403 };
                if (Picture is not null)
                {
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        if (System.IO.File.Exists(CurrentPicture))
                        {
                            System.IO.File.Delete(CurrentPicture);
                        }
                        Picture.CopyToAsync(fileStream);
                    }
                }
                return Ok();
            }
            catch (Exception err)
            {
                Console.Write(err);
                return new JsonResult("Something went wrong while updating.") { StatusCode = 500 };
            }
        }

        [HttpDelete]
        [Route("delete/{user}")]
        public IActionResult DeleteUser(string user)
        {
            string result = UserDAL.DeleteUser(user);
            if (result is "Error")
                return new JsonResult("Something went wrong while deleting the user.") { StatusCode = 500 };
            return new JsonResult("User deleted.") { StatusCode = 201 };
        }

    }
}