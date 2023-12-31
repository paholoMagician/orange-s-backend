﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using orangebackend6.Models;
using System.Data;

namespace orangebackend6.Controllers
{
    [Route("api/Usuario")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly orangeContext _context;

        public UsuarioController(orangeContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("guardarUsuario")]
        public async Task<IActionResult> guardarUsuario([FromBody] Usuario model)
        {

            if (ModelState.IsValid)
            {
                _context.Usuario.Add(model);
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

        [HttpGet("CrearCuentas/{userCod}/{ccia}/{tcuenta}")]
        public async Task<IActionResult> CrearCuentas([FromRoute] string userCod, [FromRoute] string ccia, [FromRoute] string tcuenta)
        {

            string Sentencia = " exec CrearCuenta @codUser, @codCia, @tipoCuenta ";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Sentencia, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@codUser", userCod));
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@codCia", ccia));
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@tipoCuenta", tcuenta));
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
        [Route("guardarUsuarioUbicacion")]
        public async Task<IActionResult> guardarUsuarioUbicacion([FromBody] Ubicacionuser model)
        {

            if (ModelState.IsValid)
            {
                _context.Ubicacionuser.Add(model);
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

        [HttpGet("ObtenerUsusariosGeneral")]
        public async Task<IActionResult> ObtenerUsusariosGeneral()
        {

            string Sentencia = " select * from usuario";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Sentencia, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    //adapter.SelectCommand.Parameters.Add(new SqlParameter("@coduser", ccuser));
                    adapter.Fill(dt);
                }
            }

            if (dt == null)
            {
                return NotFound("No se ha podido crear...");
            }

            return Ok(dt);


        }

        [HttpGet("ActualizarPass/{pass}/{coduser}")]
        public async Task<IActionResult> ActualizarPass([FromRoute] string pass,  [FromRoute] string coduser )
        {

            string Sentencia = " update usuario set password = @password where coduser = @cuser ";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Sentencia, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@password", pass));
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@cuser", coduser));
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
