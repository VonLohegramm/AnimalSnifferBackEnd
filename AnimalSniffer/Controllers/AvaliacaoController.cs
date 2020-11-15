using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL;
using ENTIDADE;
using Microsoft.AspNetCore.Mvc;

namespace AnimalSniffer.Controllers
{
    [Route("avaliacao")]
    [ApiController]
    public class AvaliacaoController : ControllerBase
    {
        private AVALIACAO_BLL avaliacao_bll;

        [HttpGet]
        public IActionResult CarregarAvaliacoes(ANIMAL animal)
        {
            try
            {
                avaliacao_bll = new AVALIACAO_BLL();

                return new ObjectResult(avaliacao_bll.CarregarAvaliacoesAnimal(animal));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public IActionResult InserirAvaliacao(AVALIACAO avaliacao)
        {
            try
            {
                avaliacao_bll = new AVALIACAO_BLL();

                avaliacao_bll.AvaliarAnimal(avaliacao);

                return new ObjectResult(avaliacao);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut]
        public IActionResult DesavaliarAnimal(AVALIACAO avaliacao)
        {
            try
            {
                avaliacao_bll = new AVALIACAO_BLL();

                avaliacao_bll.DesavaliarAnimal(avaliacao);

                return new ObjectResult(avaliacao);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
