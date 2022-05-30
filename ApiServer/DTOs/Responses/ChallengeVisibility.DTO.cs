using System;
using System.ComponentModel.DataAnnotations;

namespace ApiServer.DTOs.Responses
{
    public class ChallengeVisibilityDto
    {
        [Required]
        public int GroupId { get; set; }
        [Required]
        public int ChallengeId { get; set; }
    }
}