using Microsoft.AspNetCore.Mvc;
using System;
using ApiServer.DAL;
using ApiServer.DTOs.Requests;

namespace ApiServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        public UserController()
        {
        }

        [HttpGet]
        [Route("users")]
        public IActionResult GetUsers()
        {
            var users = UserDAL.GetUsers();
            if (users is null)
                return new JsonResult("Something went wrong retrieving the users.") { StatusCode = 500 };
            return Ok(users);
        }

        [HttpGet]
        [Route("friendsFrontPage/{user}")]
        public IActionResult GetFriendsFrontPage(string user)
        {
            var friends = UserDAL.GetFriendsFrontPageDB(user);
            if (friends is null)
                return new JsonResult("Something went wrong retrieving the friends.") { StatusCode = 500 };
            return Ok(friends);
        }

        [HttpPost]
        [Route("register")]
        public IActionResult RegisterUser(UserRegisterDto user)
        {
            if (ModelState.IsValid)
            {
                string result = UserDAL.RegisterUserDB(user);
                if (result is "Error")
                    return new JsonResult("Something went wrong in the registration.") { StatusCode = 500 };
                else if (result is "Taken")
                    return new JsonResult("User already exists.") { StatusCode = 409 };
                return new JsonResult("Registration completed") { StatusCode = 201 };
            }
            return new JsonResult("Invalid model for an User.") { StatusCode = 400 };
        }

        [HttpPost]
        [Route("login")]
        public IActionResult LoginUser(UserLoginDto user)
        {
            if (ModelState.IsValid)
            {
                string result = UserDAL.LoginUserDB(user);
                if (result is "Error")
                    return new JsonResult("Something went wrong in the login.") { StatusCode = 500 };
                else if (result is "NotFound")
                    return new JsonResult("User not found.") { StatusCode = 403 };
                else if (result is "WrongPass")
                    return new JsonResult("Incorrect password.") { StatusCode = 403 };
                return new JsonResult("Login successful.") { StatusCode = 200 };
            }
            return new JsonResult("Invalid model for Login.") { StatusCode = 400 };
        }

        [HttpPost]
        [Route("addActivity")]
        public IActionResult PostActivity(ActivityDto activity)
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
    }
}