using System;
using System.ComponentModel.DataAnnotations;

namespace ApiServer.DTOs.Requests
{
    public partial class UserRegisterDto
    {
        [Required]
        public string User { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName1 { get; set; }
        [Required]
        public string LastName2 { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Picture { get; set; }
        [Required]
        public string Nationality { get; set; }
    }
}