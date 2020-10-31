using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BLL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AnimalSniffer.Controllers
{
    [Route("estatistica")]
    [ApiController]
    public class EstatisticaController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        private ESTATISTICA_BLL estatistica_bll;
        public EstatisticaController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet]
        public IActionResult CarregarEstatistica()
        {
            try
            {
                estatistica_bll = new ESTATISTICA_BLL(_clientFactory);
                var array = estatistica_bll.CarregarEstatistica();

                return new ObjectResult(array);
            }
            catch (Exception e)
            {
                var exception = e.GetBaseException();
                return BadRequest(exception);
            }
        }
    }
}
