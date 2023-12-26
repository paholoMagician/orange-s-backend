using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using orangebackend6.Models;
using System.Data;

namespace orangebackend6.Controllers
{
    [Route("api/Instituto")]
    [ApiController]
    public class InstitutoController : ControllerBase
    {

        private readonly orangeContext _context;

        public InstitutoController(orangeContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("guardarInstituto")]
        public async Task<IActionResult> guardarInstituto([FromBody] Intituto model)
        {

            if (ModelState.IsValid)
            {
                _context.Intituto.Add(model);                
                if (await _context.SaveChangesAsync() > 0) {
                    return Ok(model);
                }

                else {
                    return BadRequest("Datos incorrectos");
                }

            }
            else
            {
                return BadRequest("ERROR");
            }

        }

        [HttpPut]
        [Route("ActualizarInstituto/{id}")]
        public async Task<IActionResult> ActualizarInstituto([FromRoute] int id, [FromBody] Intituto model) {
            if (id != model.Idintituto) {
                return BadRequest("No existe el equipo");
            }

            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(model);

        }

        [HttpGet("ActualizarImgInstituto/{url}/{id}")]
        public async Task<IActionResult> ActualizarImgInstituto([FromRoute] string url, [FromRoute] int id )
        {

            string Sentencia = " exec actualizarImgInstituto @urls, @idintituto ";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Sentencia, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@urls", url));
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@idintituto", id));
                    adapter.Fill(dt);
                }
            }

            if (dt == null)
            {
                return NotFound("No se ha podido crear...");
            }

            return Ok(dt);


        }

        [HttpGet("ObtenerInstituciones/{uscre}")]
        public async Task<IActionResult> ObtenerInstituciones([FromRoute] string uscre )
        {

            string Sentencia = " select * from intituto where usercrea = @usercrea ";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Sentencia, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@usercrea", uscre));
                    adapter.Fill(dt);
                }
            }

            if (dt == null)
            {
                return NotFound("No se ha podido crear...");
            }

            return Ok(dt);


        }



    }
}
