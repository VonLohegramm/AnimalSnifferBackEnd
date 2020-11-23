using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL;
using ENTIDADE;
using Microsoft.AspNetCore.Mvc;

namespace AnimalSniffer.Controllers
{
    [Route("animal")]
    [ApiController]
    public class AnimalController : ControllerBase
    {
        private ANIMAL_BLL animal_bll;

        [HttpPost]
        public IActionResult PostAnimal(ANIMAL animal)
        {
            try
            {
                animal_bll = new ANIMAL_BLL();

                animal_bll.CadastrarAnimal(animal);

                return new ObjectResult(animal);
            }
            catch (Exception e)
            {
                var exception = e.GetBaseException();
                return BadRequest(exception);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetAnimal(int id)
        {
            try
            {
                animal_bll = new ANIMAL_BLL();
                return new ObjectResult(animal_bll.CarregarAnimal(id));
            }
            catch (Exception e)
            {
                var exception = e.GetBaseException();
                return BadRequest(exception);
            }
        }

        [HttpGet()]
        public IActionResult GetAnimais()
        {
            try
            {
                animal_bll = new ANIMAL_BLL();
                return new ObjectResult(animal_bll.CarregarAnimaisAtivos());
            }
            catch (Exception e)
            {
                var exception = e.GetBaseException();
                return BadRequest(exception);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAnimal(int id)
        {
            try
            {
                animal_bll = new ANIMAL_BLL();
                animal_bll.RecolherAnimal(id);
                return Ok();
            }
            catch (Exception e)
            {
                var exception = e.GetBaseException();
                return BadRequest(exception);
            }
        }
    }
}
