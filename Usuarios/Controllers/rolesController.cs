using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Usuarios.Data;
using Usuarios.Model;

namespace ApiProgramacionIV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly usuariosContext _context;

        public RolesController(usuariosContext context)
        {
            _context = context;
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<roles>>> GetRoles()
        {
            return await _context.Roles.ToListAsync();
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<roles>> GetRolModel(int id)
        {
            var rolModel = await _context.Roles.FindAsync(id);

            if (rolModel == null)
            {
                return NotFound();
            }

            return rolModel;
        }

        // PUT: api/Roles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRolModel(int id, roles rolModel)
        {
            if (id != rolModel.IdRol)
            {
                return BadRequest();
            }

            _context.Entry(rolModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RolModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Roles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<roles>> PostRolModel(roles rolModel)
        {
            _context.Roles.Add(rolModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRolModel", new { id = rolModel.IdRol }, rolModel);
        }

        // BORRADO LOGICO
        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRolModel(int id)
        {
            var rolModel = await _context.Roles.FindAsync(id);
            if (rolModel == null)
            {
                return NotFound();
            }

            rolModel.Estado = false; // cambia el estado a 0, por lo tanto el rol esta borrado


            // actualiza y guarda los cambios en la base de datos
            _context.Roles.Remove(rolModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RolModelExists(int id)
        {
            return _context.Roles.Any(e => e.IdRol == id);
        }
    }
}
