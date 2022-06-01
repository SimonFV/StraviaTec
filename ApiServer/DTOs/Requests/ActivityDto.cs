using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ApiServer.DTOs.Requests
{
    public class ActivityDto
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public decimal Distance { get; set; }
        [Required]
        public DateTime Duration { get; set; }
        [Required]
        public string Route { get; set; }
        [Required]
        public decimal Altitude { get; set; }
        [Required]
        public DateTime Start { get; set; }
        [Required]
        public string Type { get; set; }
    }
}