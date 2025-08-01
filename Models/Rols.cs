using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RODXUsers.Models
{
    [Table("dtRols")]
    public class Rols
    {
        [Key]
        public int IdRol { get; set; }
        public string Rol { get; set; }
    }
}
