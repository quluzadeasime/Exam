using System.ComponentModel.DataAnnotations;

namespace ExamApp.DTO_s
{
    public class RegisterDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(25)]
        public string FirstName { get; set; } = null!;
        [Required]
        [MinLength(3)]
        [MaxLength(25)]
        public string Lastname { get; set; } = null!;
        [Required]
        [MinLength(8)]
        [MaxLength(40)]
        public string Username { get; set; } = null!;
        [Required]
        [MinLength(8)]
        [MaxLength(35)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
        [Required]
        [MinLength(8)]
        [MaxLength(50)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        [Required]
        [DataType(DataType.Password),Compare("Password")]
        public string ConfirmPassword { get; set; } = null!;

    }
}
