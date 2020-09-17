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

        public void CadastrarAnimal(ANIMAL animal)
        {
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
            return animal_db.SelecionarAnimaisAtivos();
        }
    }
}
