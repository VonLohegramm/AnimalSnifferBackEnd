//using System;
//using System.Collections.Generic;
//using System.Text;
//using MySql.Data.MySqlClient;
//using ENTIDADE;
//using System.Data;

//namespace DATABASE
//{
//    public class PESSOA_DB
//    {


//        public PESSOA_DB()
//        {
//            connectionMySQL = new MySqlConnection(stringBancoMySQL);
//        }

//        #region InserirPessoas
//        public void inserirPessoaBancoDados(PESSOA pessoa)
//        {
//            string insert = "insert into PESSOA (cpf, nome, id) values(@cpf, @nome, @id);";

//            try
//            {
//                Dictionary<string, object> values = new Dictionary<string, object>
//                {
//                    { "@cpf",  pessoa.CPF },
//                    { "@nome", pessoa.NOME },
//                    { "@id", pessoa.ID }
//                };

//                QueryTableMySQL(insert, values);
//            }
//            catch
//            {
//                throw;
//            }

//        }
//        #endregion

//        public List<PESSOA> selectPessoasBancoDados()
//        {
//            string select = "select * from PESSOA";
//            List<PESSOA> pessoas = new List<PESSOA>();
//            PESSOA pessoa = new PESSOA();

//            try
//            {
//                connectionMySQL.Open();

//                MySqlCommand cmd = connectionMySQL.CreateCommand();

//                cmd.CommandText = select;

//                DataTable dt = new DataTable();

//                using (MySqlDataReader reader = cmd.ExecuteReader())
//                {
//                    dt.Load(reader);
//                }

//                if(dt.Rows.Count > 0)
//                {
//                    for(int i = 0; i < dt.Rows.Count; i++)
//                    {
//                        pessoa.CPF = dt.Rows[i]["cpf"].ToString();
//                        pessoa.NOME = dt.Rows[i]["nome"].ToString();

//                        pessoas.Add(pessoa);
//                        pessoa = new PESSOA();
//                    }
//                }
//                return  pessoas;
//            }
//            catch (Exception)
//            {

//                throw;
//            }
//        }

//        public void QueryTableMySQL(string query, Dictionary<string, object> values)
//        {
//            try
//            {
//                connectionMySQL.Open();
//                transMySQL = connectionMySQL.BeginTransaction();

//                MySqlCommand cmd = connectionMySQL.CreateCommand();

//                cmd.CommandText = query;

//                foreach (string key in values.Keys)
//                {
//                    cmd.Parameters.AddWithValue(key, values[key]);
//                }

//                if (cmd.ExecuteNonQuery() > 0)
//                    transMySQL.Commit();
//                else
//                    transMySQL.Rollback();
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//        }
//    }
//}
