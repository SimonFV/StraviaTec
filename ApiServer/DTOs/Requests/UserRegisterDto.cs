using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer.DTOs.Requests
{
    [Bind]
    public class UserRegisterDto
    {
        public string User { get; set; }
        public string FirstName { get; set; }
        public string LastName1 { get; set; }
        public string LastName2 { get; set; }
        public DateTime BirthDate { get; set; }
        public string Password { get; set; }
        public IFormFile Picture { get; set; }
        public string Nationality { get; set; }
    }
}