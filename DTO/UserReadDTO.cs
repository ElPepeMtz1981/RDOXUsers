using System.ComponentModel.DataAnnotations.Schema;

namespace RODXUsers.DTO
{
    [Table("ViewUserRol")]
    public class UserReadDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public int IdRol { get; set; }
        public string Rol { get; set; }
    }
}
