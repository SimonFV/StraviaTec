using System;
using System.ComponentModel.DataAnnotations;

namespace ApiServer.DTOs.Requests
{
    public class FilePathDto
    {
        [Required]
        public string Path { get; set; }
    }
}