using System;
using System.ComponentModel.DataAnnotations;

namespace ApiServer.DTOs.Responses
{
    public class ActivityResponseDto
    {
        public int ActivityId { get; set; }
        public string UserId { get; set; }
        public Decimal Distance { get; set; }
        public TimeSpan Duration { get; set; }
        public string Route { get; set; }
        public Decimal Altitude { get; set; }
        public DateTime Start { get; set; }
        public string Type { get; set; }
    }
}