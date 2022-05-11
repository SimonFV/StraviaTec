using Microsoft.AspNetCore.Mvc;
using System;
using ApiServer.DAL;

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
    }
}