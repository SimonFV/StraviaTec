using System;

namespace ApiServer.DTOs.Responses
{
    public class ChallengeResponseDto
    {
        public int Id { get; set; }
        public string UserAdmin { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public Boolean Privacy { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Activity_Type { get; set; }
    }
}