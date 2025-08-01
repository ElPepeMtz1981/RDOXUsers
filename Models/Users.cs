using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RODXUsers.Models
{
    [Table("dtUsers")]
    public class Users
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int FKIdRol { get; set; }
        public string Password { get; set; } = string.Empty;
    }
}
