using System;
using System.ComponentModel.DataAnnotations;

namespace ApiServer.DTOs.Requests
{
    public partial class UserLoginDto
    {
        [Required]
        public string User { get; set; }
        [Required]
        public string Password { get; set; }
    }
}