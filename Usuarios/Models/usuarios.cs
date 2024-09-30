using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Usuarios.Model
{
    [Table("usuario")]
    public class usuarios
    {
        [Key]    
        public int IdUsuario {  get; set; } 
        public string Nombre { get; set; }
        public string Email { get; set; }
        public int IdRol {  get; set; } 
        public bool Estado { get; set; }

        
        [ForeignKey("IdRol")]
        public virtual roles Rol { get; set; }

    }
}
