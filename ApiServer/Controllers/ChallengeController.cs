using Microsoft.AspNetCore.Mvc;
using System;
using ApiServer.DAL;
using ApiServer.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ApiServer.Controllers
{

    /// <summary>
    /// Controller class for Challenge related http requests. 
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChallengeController : ControllerBase
    {
        public ChallengeController()
        {
        }

    /// <summary>
    /// Get all the challenges
    /// </summary>
    /// <returns><c>IActionResult</c> with the Challenges or an error message</returns>
        [HttpGet]
        [Route("challenges")]
        public IActionResult GetChallenges()
        {
            var challenges = ChallengeDAL.GetChallenges();
            if (challenges is null)
                return new JsonResult("Something went wrong") { StatusCode = 500 };

            return Ok(challenges);
        }


        /// <summary>
        /// Register Challenge participation
        /// </summary>
        /// <param name="user"><c>string</c>: User account</param>
        /// <param name="challengeId"><c>int</c>: Challenge Id to participate on</param>
        /// <returns><c>IActionResult</c> with the participation result</returns>
        [HttpPost]
        [Route("getInChallenge/{user}/{challengeId}")]
        public IActionResult GetInChallenge(string user, int challengeId)
        {
            if (ModelState.IsValid)
            {
                string result = ChallengeDAL.GetInChallenge(user, challengeId);
                if (result is "Error")
                    return new JsonResult("Something went wrong while adding the challenge.") { StatusCode = 500 };
                return new JsonResult("Challenge added.") { StatusCode = 201 };
            }
            return new JsonResult("Invalid model for User/ChallengeId.") { StatusCode = 400 };
        }

        /// <summary>
        /// Get the visibility of all the challenges
        /// </summary>
        /// <returns><c>IActionResult</c> Challenges and their visibility or an error message</returns>
        [HttpGet]
        [Route("ChallengeVisibility")]
        public IActionResult GetChallengeVisibility()
        {
            var challenges =ChallengeDAL.GetChallengeVisibility();
            if (challenges is null)
                return new JsonResult("Something went wrong retrieving the groups.") { StatusCode = 500 };
            return Ok(challenges);
        }

        /// <summary>
        /// Register a new Challenge
        /// </summary>
        /// <param name="challenge"><c>string</c>: New Challenge name</param>
        /// <returns><c>IActionResult</c> With the registration result</returns>
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


        /// <summary>
        /// Gets all the challenges to which the user is subscribed
        /// </summary>
        /// <param name="user"><c>string</c>: User account</param>
        /// <returns><c>IActionResult</c>: Challenges to which the user is subscribed</returns>
        [HttpGet]
        [Route("challengesByUser/{user}")]
        public IActionResult GetChallengesByUser(string user)
        {
            var challenges = ChallengeDAL.GetChallengesByUser(user);
            if (challenges is null)
                return new JsonResult("Something went wrong") { StatusCode = 500 };

            return Ok(challenges);
        }

        /// <summary>
        /// Add an Activity to a Challenge
        /// </summary>
        /// <param name="challengeId"><c>int</c>: Challenge Id</param>
        /// <param name="activityId"><c>int</c>: Activity Id</param>
        /// <returns><c>IActionResult</c> with the binding result</returns>
        [HttpPost]
        [Route("AddChallengeActivity/{challengeId}/{activityId}")]
        public IActionResult AddChallengeActivity(int challengeId, int activityId)
        {
            if (ModelState.IsValid)
            {
                string result = ChallengeDAL.AddChallengeActivity(challengeId, activityId);
                if (result is "Error")
                    return new JsonResult("Something went wrong while adding the challenge.") { StatusCode = 500 };
                return new JsonResult("Activity added.") { StatusCode = 201 };
            }
            return new JsonResult("Invalid model for ChallengeId/ActId.") { StatusCode = 400 };
        }


        [HttpGet]
        [Route("ChallengeProgress/{challengeId}/{user}")]
        public IActionResult ChallengeProgress(int challengeId, string user)
        {

            var result = ChallengeDAL.GetChallengProgres(challengeId, user);
            if (result is -1)
                return new JsonResult("Something went wrong") { StatusCode = 500 };
            return Ok(result);
        }
    }
}