using System;
using System.ComponentModel.DataAnnotations;

namespace ApiMongo.DTOs
{
    public class CommentDto
    {
        [Required]
        public string User { get; set; }
        [Required]
        public int ActivityId { get; set; }
        [Required]
        public DateTime PostTime { get; set; }
        [Required]
        public string Body { get; set; }
    }
}