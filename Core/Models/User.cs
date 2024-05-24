using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class User:IdentityUser
    {
        [Required]
        [MinLength(3)]
        [MaxLength(25)]
        public string Name { get; set; } = null!;
        [Required]
        [MinLength(3)]
        [MaxLength(25)]
        public string Surname { get; set; } = null!;

    }
}
