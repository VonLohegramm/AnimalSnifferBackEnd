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
            if (string.IsNullOrEmpty(animal.LATITUDE))
                throw new Exception("Informe a latitude");

            if (string.IsNullOrEmpty(animal.LONGITUDE))
                throw new Exception("Informe a longitude");

            if (string.IsNullOrEmpty(animal.RACA))
                throw new Exception("Informe a raça do animal");

            if (string.IsNullOrEmpty(animal.TIPO))
                throw new Exception("Informe o tipo de animal");

            if (animal.IDUSUARIO == 0)
                throw new Exception("Informe o usuário(ID) que está fazendo o cadastrado");

            animal_db.InserirAnimal(animal);
        }

        public void RecolherAnimal(int id)
        {
            if (id == 0)
                throw new Exception("Informe o id do animal");

            animal_db.RecolherAnimal(id);
        }

        public ANIMAL CarregarAnimal(int id)
        {
            if(id == 0)
                throw new Exception("Informe o id do animal");

            ANIMAL animal = animal_db.SelecionarAnimal(id);

            animal.AVALIACOES = avaliacao_bll.CarregarAvaliacoesAnimal(animal);

            return animal;
        }

        public List<ANIMAL> CarregarAnimaisAtivos()
        {
            List<ANIMAL> animais = animal_db.SelecionarAnimaisAtivos();
            
            foreach(var animal in animais)
            {
                animal.AVALIACOES = avaliacao_bll.CarregarAvaliacoesAnimal(animal);
            }

            return animais;
        }

        public List<ANIMAL> CarregarAnimais()
        {
            List<ANIMAL> animais = animal_db.SelecionarAnimais();

            return animais;
        }
    }
}
