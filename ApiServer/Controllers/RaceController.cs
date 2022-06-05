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


        [HttpGet]
        [Route("racesByUser/{user}")]
        public IActionResult GetRacesByUser(string user)
        {
            var races = RaceDAL.GetRacesByUser(user);
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
        [Route("Participants/{Id}")]
        public IActionResult GetRaceParticipants(int Id)
        {
            var participants = RaceDAL.PARTICIPANTS_IN_RACE(Id);
            if (participants is null)
                return new JsonResult("Something went wrong retrieving the groups.") { StatusCode = 500 };
            return Ok(participants);
        }

        [HttpGet]
        [Route("Record/{Id}")]
        public IActionResult GetRaceRecord(int Id)
        {
            var records = RaceDAL.RECORD_IN_RACE(Id);
            if (records is null)
                return new JsonResult("Something went wrong retrieving the groups.") { StatusCode = 500 };
            return Ok(records);
        }

        [HttpGet]
        [Route("CategoryRace/{Id}")]
        public IActionResult GetCategoryRace(int Id)
        {
            var categories = RaceDAL.GetCategoryRace(Id);
            if (categories is null)
                return new JsonResult("Something went wrong retrieving the groups.") { StatusCode = 500 };
            return Ok(categories);
        }

        [HttpGet]
        [Route("RaceRegister/{User}/{id}/{optionselect}")]
        public IActionResult RaceRegister(string User,int id, string optionselect)
        {
            var categories = RaceDAL.RaceRegister(User, id, optionselect);
            if (categories is null)
                return new JsonResult("Something went wrong retrieving the groups.") { StatusCode = 500 };
            return Ok(categories);
        }

        [HttpGet]
        [Route("ToPay/{User}")]
        public IActionResult RaceToPay(string User)
        {
            var categories = RaceDAL.RaceToPay(User);
            if (categories is null)
                return new JsonResult("Something went wrong retrieving the groups.") { StatusCode = 500 };
            return Ok(categories);
        }

        [HttpGet]
        [Route("Pay/{id}/{user}/{payment}")]
        public IActionResult RacePay(int id,string user, string payment)
        {
            var categories = RaceDAL.RacePay(id,user,payment);
            if (categories is null)
                return new JsonResult("Something went wrong retrieving the groups.") { StatusCode = 500 };
            return Ok(categories);
        }
    }
}