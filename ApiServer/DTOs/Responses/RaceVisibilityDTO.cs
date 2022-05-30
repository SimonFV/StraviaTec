using System;
using System.ComponentModel.DataAnnotations;

namespace ApiServer.DTOs.Responses
{
    public class RaceVisibilityDto
    {
        [Required]
        public int GroupId { get; set; }
        [Required]
        public int RaceId { get; set; }
    }
}