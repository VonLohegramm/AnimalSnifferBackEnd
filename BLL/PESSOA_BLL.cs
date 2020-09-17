//using DATABASE;
//using ENTIDADE;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;

//namespace BLL
//{
//    public class PESSOA_BLL
//    {
//        PESSOA_DB pessoadb = new PESSOA_DB();

//        public void inserirPessoa(PESSOA pessoa)
//        {
//            if (string.IsNullOrEmpty(pessoa.CPF))
//            {
//                throw new Exception("Informe o cpf");
//            }
//            if (string.IsNullOrEmpty(pessoa.NOME))
//            {
//                throw new Exception("Informe o nome");
//            }

//            pessoadb.inserirPessoaBancoDados(pessoa);
//        }

//        public List<PESSOA> pegarPessoas()
//        {
//            return pessoadb.selectPessoasBancoDados();
//        }
//    }
//}
