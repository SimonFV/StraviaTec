using System;
using System.ComponentModel.DataAnnotations;

namespace ApiServer.DTOs.Requests
{
    public partial class RaceRegisterDto
    {
        [Required]
        public string UserAdmin { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Route { get; set; }
        [Required]
        public float Cost { get; set; }
        [Required]
        public Boolean Privacy { get; set; }
        public string Groups { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public string Type { get; set; }
    }
}