//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using ENTIDADE;
//using BLL;

//namespace Tutorial2.Controllers
//{
//    [Route("pessoa")]
//    [ApiController]
//    public class PessoaController : ControllerBase
//    {
//        PESSOA_BLL pessoa_bll;

//        [HttpPost()]
//        public IActionResult PostPessoa(PESSOA pessoa)
//        {
//            try
//            {
//                pessoa_bll = new PESSOA_BLL();
//                pessoa_bll.inserirPessoa(pessoa);

//                return new ObjectResult(pessoa);
//            }
//            catch (Exception e)
//            {
//                var exception = e.GetBaseException();
//                return BadRequest(exception);
//            }
//        }

//        [HttpGet("get")]
//        public IActionResult GetPessoas()
//        {
//            try
//            {
//                pessoa_bll = new PESSOA_BLL();
//                List<PESSOA> pessoas = pessoa_bll.pegarPessoas();

//                return new ObjectResult(pessoas);
//            }
//            catch (Exception e)
//            {
//                var exception = e.GetBaseException();
//                return BadRequest(exception);
//            }
//        }

//        [HttpPut("put/{cpf}")]
//        public IActionResult PutPessoa(string cpf, [FromBody] PESSOA pessoa)
//        {
//            try
//            {
//                Console.WriteLine("lalala");
//                return new ObjectResult("Deu CERTO");
//            }
//            catch (Exception e)
//            {
//                var exception = e.GetBaseException();
//                return BadRequest(exception);
//            }
//        }
//    }
//}