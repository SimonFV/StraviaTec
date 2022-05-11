using Microsoft.AspNetCore.Mvc;
using System;
using ApiServer.DAL;
using ApiServer.DTOs.Requests;

namespace api.Controllers
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
                return new JsonResult("Something went wrong") { StatusCode = 500 };

            return Ok(users);
        }

        [HttpPost]
        [Route("users")]
        public IActionResult RegisterUser(UserRegisterDto user)
        {
            if (ModelState.IsValid)
            {
                var result = UserDAL.RegisterUserDB(user);
                if (result is "Error")
                {
                    return new JsonResult("Something went wrong in the registration.") { StatusCode = 500 };
                }
                else if (result is "Taken")
                {
                    return new JsonResult("User already exists.") { StatusCode = 409 };
                }
                return new JsonResult("Registration completed") { StatusCode = 201 };
            }
            return new JsonResult("Invalid model for an User.") { StatusCode = 400 };
        }
    }
}