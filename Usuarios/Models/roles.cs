using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Usuarios.Model
{
    [Table("rol")]
    public class roles
    {
        [Key]
        public int IdRol { get; set; }
        public string NombreRol { get; set; }
        public bool Estado { get; set; }    


    }
}
