using System.ComponentModel.DataAnnotations;

namespace RODXUsers.DTO
{
    public class PasswordUpdateDto
    {
        [Required]
        public string NewPassword { get; set; }
    }
}
