using System.ComponentModel.DataAnnotations;

namespace RODXUsers.DTO
{
    public class UserUpdateDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int FkIdRol { get; set; }
    }
}
