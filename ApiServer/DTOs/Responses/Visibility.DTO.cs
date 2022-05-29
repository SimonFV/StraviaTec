using System;
using System.ComponentModel.DataAnnotations;

namespace ApiServer.DTOs.Responses
{
    public class VisibilityrDto
    {
        [Required]
        public int GroupId { get; set; }
        [Required]
        public int ChallengeId { get; set; }
    }
}