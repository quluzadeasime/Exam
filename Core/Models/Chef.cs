using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Chef:BaseEntity
    {
        [Required]
        [MinLength(5)]
        [MaxLength(45)]
        public string Fullname { get; set; } = null!;
        [Required]
        [MinLength(10)]
        [MaxLength(100)]
        public string Description { get; set; } = null!;
        public string? ImgUrl { get; set; }
        [NotMapped]
        public IFormFile? PhotoFile { get; set; }

    }
}
