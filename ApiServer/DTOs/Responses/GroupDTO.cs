using System;

namespace ApiServer.DTOs.Responses
{
    public class GroupDTO
    {
        public int Id { get; set; }
        public string AdminUser { get; set; }
        public string Name { get; set; }
    }
}