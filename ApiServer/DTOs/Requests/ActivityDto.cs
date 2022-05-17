using System;
using System.ComponentModel.DataAnnotations;

namespace ApiServer.DTOs.Requests
{
    public class ActivityDto
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public float Distance { get; set; }
        [Required]
        public DateTime Duration { get; set; }
        [Required]
        public string Route { get; set; }
        [Required]
        public float Altitude { get; set; }
        [Required]
        public DateTime Start { get; set; }
        [Required]
        public string Type { get; set; }
    }
}