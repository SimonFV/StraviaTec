using System;
using System.ComponentModel.DataAnnotations;

namespace ApiServer.DTOs.Responses
{
    public class GroupDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string AdminUser { get; set; }
        [Required]
        public string Name { get; set; }
    }
}