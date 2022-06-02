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
    public class RaceController : ControllerBase
    {
        public RaceController()
        {
        }

        [HttpGet]
        [Route("races")]
        public IActionResult GetRaces()
        {
            var races = RaceDAL.GetRaces();
            if (races is null)
                return new JsonResult("Something went wrong") { StatusCode = 500 };

            return Ok(races);
        }

        [HttpPost]
        [Route("races")]
        public IActionResult RegisterRace(RaceRegisterDto race)
        {
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

        [HttpGet]
        [Route("RaceVisibility")]
        public IActionResult GetRaceVisibility()
        {
            var races =RaceDAL.GetRaceVisibility();
            if (races is null)
                return new JsonResult("Something went wrong retrieving the groups.") { StatusCode = 500 };
            return Ok(races);
        }

        [HttpGet]
        [Route("Participants/{name}")]
        public IActionResult GetRaceParticipants(string name)
        {
            var participants = RaceDAL.PARTICIPANTS_IN_RACE(name);
            if (participants is null)
                return new JsonResult("Something went wrong retrieving the groups.") { StatusCode = 500 };
            return Ok(participants);
        }

        [HttpGet]
        [Route("Record/{name}")]
        public IActionResult GetRaceRecord(string name)
        {
            var records = RaceDAL.RECORD_IN_RACE(name);
            if (records is null)
                return new JsonResult("Something went wrong retrieving the groups.") { StatusCode = 500 };
            return Ok(records);
        }
    }
}