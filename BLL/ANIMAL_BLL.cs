using DATABASE;
using ENTIDADE;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class ANIMAL_BLL
    {
        private ANIMAL_DB animal_db = new ANIMAL_DB();
        private AVALIACAO_BLL avaliacao_bll = new AVALIACAO_BLL();

        public void CadastrarAnimal(ANIMAL animal)
        {
            if (animal.LATITUDE == null)
                throw new Exception("Informe a latitude");

            if (animal.LONGITUDE == null)
                throw new Exception("Informe a longitude");

            if (string.IsNullOrEmpty(animal.RACA))
                throw new Exception("Informe a raça do animal");

            if (string.IsNullOrEmpty(animal.TIPO))
                throw new Exception("Informe o tipo de animal");

            if (animal.IDUSUARIO == 0)
                throw new Exception("Informe o usuário(ID) que está fazendo o cadastrado");

            animal_db.InserirAnimal(animal);
        }

        public void RecolherAnimal(ANIMAL animal)
        {
            if (animal.ID == 0)
                throw new Exception("Informe o id do animal");

            animal_db.RecolherAnimal(animal);
        }

        public ANIMAL CarregarAnimal(int id)
        {
            if(id == 0)
                throw new Exception("Informe o id do animal");

            return animal_db.SelecionarAnimal(id);
        }

        public List<ANIMAL> CarregarAnimais()
        {
            List<ANIMAL> animais = animal_db.SelecionarAnimaisAtivos();
            
            foreach(var animal in animais)
            {
                animal.AVALIACOES = avaliacao_bll.CarregarAvaliacoesAnimal(animal);
            }

            return animais;
        }
    }
}
