using System;

namespace ApiServer.DTOs.Responses
{
    public class FriendsFrontPage
    {
        public string User { get; set; }
        public string FirstName { get; set; }
        public string LastName1 { get; set; }
        public string LastName2 { get; set; }
        public string Type { get; set; }
        public DateTime Start { get; set; }
        public string Route { get; set; }
        public decimal Distance { get; set; }
    }
}