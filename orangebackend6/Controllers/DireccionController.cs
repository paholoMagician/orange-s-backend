using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using orangebackend6.Models;

namespace orangebackend6.Controllers
{
    [Route("api/Direccion")]
    [ApiController]
    public class DireccionController : ControllerBase
    {

        private readonly orangeContext _context;

        public DireccionController(orangeContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("guardarDireccion")]
        public async Task<IActionResult> guardarDireccion([FromBody] Direcciones model)
        {

            if (ModelState.IsValid)
            {
                _context.Direcciones.Add(model);
                if (await _context.SaveChangesAsync() > 0)
                {
                    return Ok(model);
                }

                else
                {
                    return BadRequest("Datos incorrectos");
                }

            }
            else
            {
                return BadRequest("ERROR");
            }
        }

    }
}
