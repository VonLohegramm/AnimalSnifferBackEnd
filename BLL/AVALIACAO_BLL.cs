using DATABASE;
using ENTIDADE;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class AVALIACAO_BLL
    {
        private AVALIACAO_DB avaliacao_db = new AVALIACAO_DB();

        public List<AVALIACAO> CarregarAvaliacoesAnimal(ANIMAL animal)
        {
            if(animal.ID == 0)
                throw new Exception("Informe o id do Animal");

            return avaliacao_db.SelecionarAvalicoes(animal.ID);
        }

        public void AvaliarAnimal(AVALIACAO avaliacao)
        {
            if (avaliacao.IDANIMAL == 0)
                throw new Exception("Informe o id do Animal");

            if (avaliacao.IDUSUARIO == 0)
                throw new Exception("Informe o id do Usuario");

            avaliacao_db.InserirAvaliacao(avaliacao);
        }

        public void DesavaliarAnimal(AVALIACAO avaliacao)
        {
            if (avaliacao.IDANIMAL == 0)
                throw new Exception("Informe o id do Animal");

            if (avaliacao.IDUSUARIO == 0)
                throw new Exception("Informe o id do Usuario");

            avaliacao_db.RemoverAvaliacao(avaliacao);
        }
    }
}
