using System;

namespace ApiServer.DTOs.Responses
{
    public partial class RaceResponseDto
    {
        public int Id { get; set; }
        public string UserAdmin { get; set; }
        public string Name { get; set; }
        public string Route { get; set; }
        public decimal Cost { get; set; }
        public Boolean Privacy { get; set; }
        public DateTime StartDate { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
    }
}  