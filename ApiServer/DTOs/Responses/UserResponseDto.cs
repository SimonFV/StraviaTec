using System;

namespace ApiServer.DTOs.Responses
{
    public partial class UserResponseDto
    {
        public string User { get; set; }
        public string FirstName { get; set; }
        public string LastName1 { get; set; }
        public string LastName2 { get; set; }
        public DateTime BirthDate { get; set; }
        public string Picture { get; set; }
        public string Nationality { get; set; }
    }
}