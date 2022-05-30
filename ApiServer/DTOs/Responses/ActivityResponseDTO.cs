using System;
using System.ComponentModel.DataAnnotations;

namespace ApiServer.DTOs.Requests
{
    public class ActivityResponseDto
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public Decimal Distance { get; set; }
        [Required]
        public TimeSpan Duration { get; set; }
        [Required]
        public string Route { get; set; }
        [Required]
        public Decimal Altitude { get; set; }
        [Required]
        public DateTime Start { get; set; }
        [Required]
        public string Type { get; set; }
    }
}