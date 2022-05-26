using Microsoft.AspNetCore.Mvc;
using System;
using ApiServer.DAL;
using ApiServer.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ApiServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChallengeController : ControllerBase
    {
        public ChallengeController()
        {
        }

        [HttpGet]
        [Route("challenges")]
        public IActionResult GetChallenges()
        {
            var challenges = ChallengeDAL.GetChallenges();
            if (challenges is null)
                return new JsonResult("Something went wrong") { StatusCode = 500 };

            return Ok(challenges);
        }

        [HttpPost]
        [Route("challenges")]
        public IActionResult RegisterChallenge(ChallengeRegisterDto challenge)
        {
            if (ModelState.IsValid)
            {
                var result = ChallengeDAL.RegisterChallengeDB(challenge);
                if (result is "Error")
                {
                    return new JsonResult("Something went wrong in the registration.") { StatusCode = 500 };
                }
                else if (result is "Already Exists")
                {
                    return new JsonResult("Challenge already exists.") { StatusCode = 409 };
                }
                else if (result is "Activity Type not found")
                {
                    return new JsonResult("Activity Type not found") { StatusCode = 408 };
                }
                return new JsonResult("Registration completed") { StatusCode = 201 };
            }
            return new JsonResult("Invalid model for an User.") { StatusCode = 400 };
        }
    }
}