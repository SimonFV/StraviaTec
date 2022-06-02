using System;

namespace ApiServer.DTOs.Responses
{
    public partial class RaceParticipantsDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Category { get; set; }
    }
}  