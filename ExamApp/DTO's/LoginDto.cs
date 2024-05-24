using System.ComponentModel.DataAnnotations;

namespace ExamApp.DTO_s
{
    public class LoginDto
    {
        [Required]
        [MinLength(8)]
        [MaxLength(50)]
        public string UsernameOrEmail { get; set; } = null!;
        [Required]
        [MinLength(8)]
        [MaxLength(50)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        public bool RememberMe {  get; set; }
    }
}
