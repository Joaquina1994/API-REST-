using Usuarios.Data;
using Usuarios.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace ApiProgramacionIV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly usuariosContext _context;

        public UsuariosController(usuariosContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            try
            {
                List<usuarios> usuarios = await _context.Usuarios
                    .Include(usuario => usuario.Rol)
                    .ToListAsync();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hubo un problema, error: {ex.Message}");
            }
        }

        [HttpGet("{id_Usuario}")]
        public async Task<IActionResult> GetUsuariosById(int id_Usuario)
        {
            try
            {
                usuarios usuario = await _context.Usuarios
                    .Include(usuario => usuario.Rol)
                    .FirstOrDefaultAsync(u => u.IdUsuario == id_Usuario);

                if (usuario is null)
                {
                    Log.Error($"No existe el usuario con el id {id_Usuario}");
                    return NotFound("No existe el usuario indicado.");
                }

                Log.Information("Se llamó al endpoint GetUsuariosById");
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                Log.Error($"Hubo un problema en GetUsuariosById, error: {ex.Message}");
                return BadRequest($"Hubo un problema, error: {ex.Message}");
            }
        }

        [HttpGet("UsuariosPorRoles")]
        public async Task<IActionResult> GetUsuariosPorRoles()
        {
            try
            {
                var usuariosPorRoles = await _context.Usuarios
                    .Include(u => u.Rol)
                    .GroupBy(r => new {r.Nombre,  r.Rol.NombreRol }) // agrupa por nombre y rol 
                    .Select(s => new
                    {
                        Nombre = s.Key.Nombre, 
                        Nombre_Rol = s.Key.NombreRol,
                        Cantidad_Usuarios = s.Count()
                    })
                    .OrderByDescending(order => order.Cantidad_Usuarios)
                        //.ThenByDescending(order => order.Edad)
                    .ToListAsync();
                return Ok(usuariosPorRoles);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("UsuariosPorRol")]
        public async Task<IActionResult> GetUsuariosPorRol(string nombre_Rol)
        {
            try
            {
                roles roles = await _context.Roles
                    .FirstOrDefaultAsync(r => r.NombreRol == nombre_Rol);

                if (roles is null)
                    return NotFound($"No existe el rol indicado {nombre_Rol}");

                List<usuarios> usuarios = await _context.Usuarios
                    .Include(usuario => usuario.Rol)
                    .Where(r => r.Rol.NombreRol == nombre_Rol)
                    .ToListAsync();

                if (!usuarios.Any())
                    return NotFound($"No existen usuarios del tipo {nombre_Rol}");

                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        /* 
         * cambie el codigo y le agregue validaciones porque me cambiaba el idrol, y me creaba un nuevo rol
         */

        [HttpPost]
        public async Task<IActionResult> CreateUsuario(usuarios usuarioModel)
        {
            try
            {
                // Busca si cuando el usuario se crea, el rol que intenta asignar existe
                var existeRol = await _context.Roles.FindAsync(usuarioModel.IdRol);

                if (existeRol == null)
                {
                    return BadRequest("El rol asignado no existe.");
                }

                // Asigna el rol existente al usuario
                usuarioModel.Rol = existeRol;

                _context.Usuarios.Add(usuarioModel);
                await _context.SaveChangesAsync();
                return Ok(usuarioModel);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hubo un problema, error: {ex.Message}");
            }
        }




        [HttpPut]
        public async Task<IActionResult> UpdateUsuario(usuarios usuarioModel)
        {
            usuarios usuario = await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.IdUsuario == usuarioModel.IdUsuario);

            if (usuario is null)
            {
                return NotFound("No existe el usuario indicado");
            }

            try
            {
                _context.Entry(usuarioModel).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok("Se modificaron los datos del usuario.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        // BORRADO FISICO
        /*[HttpDelete("{id_Usuario}")]
        public async Task<IActionResult> DeleteUsuario(int id_Usuario)
        {
            usuarios usuario = await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.IdUsuario == id_Usuario);

            if (usuario is null)
                return NotFound($"No existe el usuario con ID: {id_Usuario}");

            try
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();

                return Ok("Se eliminó el registro indicado.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }*/

        // BORRADO LOGICO
        [HttpDelete("{id_Usuario}")]
        public async Task<IActionResult> DeleteUsuario(int id_Usuario)
        {
            usuarios usuario = await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.IdUsuario == id_Usuario);

            if (usuario is null)
                return NotFound($"No existe el usuario con ID: {id_Usuario}");

            try
            {
                usuario.Estado = false; // cambia el estado del usuario a 0, por lo tanto esta borrado


                // actualiza y guarda los cambios en la base de datos
                _context.Usuarios.Update(usuario);
                await _context.SaveChangesAsync();

                return Ok("Se eliminó el registro indicado.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
