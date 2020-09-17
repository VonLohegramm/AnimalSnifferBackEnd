using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL;
using ENTIDADE;
using Microsoft.AspNetCore.Mvc;

namespace AnimalSniffer.Controllers
{
    [Route("usuario")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        USUARIO_BLL usuario_bll;

        [HttpPost]
        public IActionResult PostUsuario(USUARIO usuario)
        {
            try
            {
                usuario_bll = new USUARIO_BLL();
                usuario_bll.InserirUsuario(usuario);

                return new ObjectResult(usuario);
            }
            catch (Exception e)
            {
                var exception = e.GetBaseException();
                return BadRequest(exception);
            }
        }

        [HttpGet("Login")]
        public IActionResult LoginUsuario(USUARIO usuario)
        {
            try
            {
                usuario_bll = new USUARIO_BLL();
                return new ObjectResult(usuario_bll.Login(usuario));
            }
            catch (Exception e)
            {
                var exception = e.GetBaseException();
                return BadRequest(exception);
            }
        }
    }
}
