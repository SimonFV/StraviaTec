using Microsoft.AspNetCore.Mvc;
using System;
using ApiServer.DAL;
using ApiServer.DTOs.Requests;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChallengeController : ControllerBase
    {
        public ChallengeController()
        {
        }

        [HttpGet]
        [Route("challenges")]
        public IActionResult GetUsers()
        {
            var challenges = ChallengeDAL.GetChallenges();
            if (challenges is null)
                return new JsonResult("Something went wrong") { StatusCode = 500 };

            return Ok(challenges);
        }
/*
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
        }*/
    }
}