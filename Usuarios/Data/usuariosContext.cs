using Microsoft.EntityFrameworkCore;
using Usuarios.Model;

namespace Usuarios.Data
{
    public class usuariosContext : DbContext
    {
        public usuariosContext(DbContextOptions<usuariosContext> options) : base(options) { }

        public DbSet<usuarios> Usuarios { get; set; }
        public DbSet<roles> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<usuarios>().HasKey(usuario => usuario.IdUsuario);
            modelBuilder.Entity<roles>().HasKey(rol => rol.IdRol);

            base.OnModelCreating(modelBuilder);
        }
    }
}
