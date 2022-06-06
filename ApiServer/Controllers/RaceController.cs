using Microsoft.AspNetCore.Mvc;
using System;
using ApiServer.DAL;
using ApiServer.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ApiServer.Controllers
{
    /// <summary>
    /// Controller for races
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RaceController : ControllerBase
    {
        public RaceController()
        {
        }
        /// <summary>
        /// Returns all Races
        /// </summary>
        /// <returns>Races</returns>
        [HttpGet]
        [Route("races")]
        public IActionResult GetRaces()
        {
            var races = RaceDAL.GetRaces();
            if (races is null)
                return new JsonResult("Something went wrong") { StatusCode = 500 };

            return Ok(races);
        }

        /// <summary>
        /// Races by user
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Races</returns>
        [HttpGet]
        [Route("racesByUser/{user}")]
        public IActionResult GetRacesByUser(string user)
        {
            var races = RaceDAL.GetRacesByUser(user);
            if (races is null)
                return new JsonResult("Something went wrong") { StatusCode = 500 };

            return Ok(races);
        }

        /// <summary>
        /// Register in a race
        /// </summary>
        /// <param name="race"></param>
        /// <returns>Result</returns>
        [HttpPost]
        [Route("reraces")]
        public IActionResult RegisterRace(RaceRegisterDto race)
        {
            Console.Write(race);
            if (ModelState.IsValid)
            {
                var result = RaceDAL.RegisterRaceDB(race);
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
        /// Requests the list of races available.
        /// </summary>
        /// <returns>Races</returns>
        [HttpGet]
        [Route("RaceVisibility")]
        public IActionResult GetRaceVisibility()
        {
            var races = RaceDAL.GetRaceVisibility();
            if (races is null)
                return new JsonResult("Something went wrong retrieving the groups.") { StatusCode = 500 };
            return Ok(races);
        }

        /// <summary>
        /// Requests the list of participants
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>Participants</returns>
        [HttpGet]
        [Route("Participants/{Id}")]
        public IActionResult GetRaceParticipants(int Id)
        {
            var participants = RaceDAL.PARTICIPANTS_IN_RACE(Id);
            if (participants is null)
                return new JsonResult("Something went wrong retrieving the groups.") { StatusCode = 500 };
            return Ok(participants);
        }

        /// <summary>
        /// Requests the list of records.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>Records</returns>
        [HttpGet]
        [Route("Record/{Id}")]
        public IActionResult GetRaceRecord(int Id)
        {
            var records = RaceDAL.RECORD_IN_RACE(Id);
            if (records is null)
                return new JsonResult("Something went wrong retrieving the groups.") { StatusCode = 500 };
            return Ok(records);
        }


        /// <summary>
        /// Requests the categories of a race.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>Categories.</returns>
        [HttpGet]
        [Route("CategoryRace/{Id}")]
        public IActionResult GetCategoryRace(int Id)
        {
            var categories = RaceDAL.GetCategoryRace(Id);
            if (categories is null)
                return new JsonResult("Something went wrong retrieving the groups.") { StatusCode = 500 };
            return Ok(categories);
        }

        /// <summary>
        /// Request for registering an user to a race.
        /// </summary>
        /// <param name="User"></param>
        /// <param name="id"></param>
        /// <param name="optionselect"></param>
        /// <returns>Result</returns>
        [HttpGet]
        [Route("RaceRegister/{User}/{id}/{optionselect}")]
        public IActionResult RaceRegister(string User, int id, string optionselect)
        {
            var categories = RaceDAL.RaceRegister(User, id, optionselect);
            if (categories is null)
                return new JsonResult("Something went wrong retrieving the groups.") { StatusCode = 500 };
            return Ok(categories);
        }

        /// <summary>
        /// Request for races to pay.
        /// </summary>
        /// <param name="User"></param>
        /// <returns>Categories</returns>
        [HttpGet]
        [Route("ToPay/{User}")]
        public IActionResult RaceToPay(string User)
        {
            var categories = RaceDAL.RaceToPay(User);
            if (categories is null)
                return new JsonResult("Something went wrong retrieving the groups.") { StatusCode = 500 };
            return Ok(categories);
        }

        /// <summary>
        /// Request for paying the suscription to a race.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <param name="payment"></param>
        /// <returns>Result categories</returns>
        [HttpGet]
        [Route("Pay/{id}/{user}/{payment}")]
        public IActionResult RacePay(int id, string user, string payment)
        {
            var categories = RaceDAL.RacePay(id, user, payment);
            if (categories is null)
                return new JsonResult("Something went wrong retrieving the groups.") { StatusCode = 500 };
            return Ok(categories);
        }
    }
}