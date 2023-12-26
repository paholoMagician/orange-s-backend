using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using orangebackend6.Models;
using System.Data;

namespace orangebackend6.Controllers
{
    [Route("api/Perfil")]
    [ApiController]
    public class PerfilController : ControllerBase
    {

        private readonly orangeContext _context;
        public PerfilController(orangeContext context)
        {
            _context = context;
        }

        [HttpGet("ObtenerPerfil/{userCod}")]
        public async Task<IActionResult> ObtenerPerfil([FromRoute] string userCod)
        {

            string Sentencia = " exec ObtenerPerfil @usCod ";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Sentencia, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@usCod", userCod));
                    adapter.Fill(dt);
                }
            }

            if (dt == null)
            {
                return NotFound("No se encontro este WebUser...");
            }

            return Ok(dt);

        }
        
        [HttpGet("ActualizarImgPerfil/{urlImg}/{userCod}")]
        public async Task<IActionResult> ActualizarImgPerfil([FromRoute] string urlImg, [FromRoute] string userCod)
        {

            string Sentencia = " exec actualizarImgPerfil @url, @usCod ";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Sentencia, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@url", urlImg));
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@usCod", userCod));
                    adapter.Fill(dt);
                }
            }

            if (dt == null)
            {
                return NotFound("No se encontro este WebUser...");
            }

            return Ok(dt);

        }

        [HttpPost]
        [Route("guardarPerfil")]
        public async Task<IActionResult> guardarPerfil([FromBody] Perfil model)
        {

            if (ModelState.IsValid)
            {
                _context.Perfil.Add(model);
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

        [HttpPut]
        [Route("ActualizarPerfil/{coduser}")]
        public async Task<IActionResult> ActualizarPerfil([FromRoute] string coduser, [FromBody] Perfil model)
        {
            if (coduser != model.Coduser)
            {
                return BadRequest("No existe el usuario");
            }
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(model);
        }


    }
}
