using System;
using System.ComponentModel.DataAnnotations;

namespace ApiServer.DTOs.Requests
{
    public class ChallengeRegisterDto
    {
        [Required]
        public string User { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Class { get; set; }
        [Required]
        public Boolean Privacy { get; set; }
        public string Groups { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public string Activity_Type { get; set; }

        [Required]
        public Decimal Objective { get; set; }
    }
}