using ENTIDADE;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DATABASE
{
    public class AVALIACAO_DB
    {
        #region Atributos
        private MySqlConnection connectionMySQL;
        private MySqlTransaction transMySQL;
        #endregion

        #region Constructor
        public AVALIACAO_DB()
        {
            connectionMySQL = DBCONFIG.OpenConnection();
        }
        #endregion

        #region Selecionar avaliacoes do animal
        public List<AVALIACAO> SelecionarAvalicoes(int idAnimal)
        {
            string select = "select * from AVALIACAO where idAnimal = @idAnimal";

            List<AVALIACAO> avaliacoes = new List<AVALIACAO>();
            AVALIACAO avaliacao = new AVALIACAO();

            try
            {
                Dictionary<string, object> values = new Dictionary<string, object>
                {
                    { "@idAnimal",  idAnimal }
                };

                DataTable dt = SelectTableMySQL(select, values);

                if (dt.Rows.Count > 0)
                {
                    foreach(DataRow row in dt.Rows)
                    {
                        avaliacao.IDUSUARIO = Convert.ToInt32(row["idUsuario"].ToString());
                        avaliacao.IDANIMAL = Convert.ToInt32(row["idAnimal"].ToString());

                        avaliacoes.Add(avaliacao);
                        avaliacao = new AVALIACAO();
                    }
                }

                return avaliacoes;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region InserirAvaliacao
        public void InserirAvaliacao(AVALIACAO avaliacao)
        {
            string insert = "insert into AVALIACAO(idUsuario, idAnimal) values(@idUsuario, @idAnimal);";

            try
            {
                Dictionary<string, object> values = new Dictionary<string, object>
                {
                    { "@idUsuario", avaliacao.IDUSUARIO },
                    { "@idAnimal", avaliacao.IDANIMAL }
                };

                QueryTableMySQL(insert, values);
            }
            catch
            {
                throw;
            }

        }
        #endregion

        #region RemoverAvaliacao
        public void RemoverAvaliacao(AVALIACAO avaliacao)
        {
            string delete = "delete from AVALIACAO where idUsuario = @idUsuario and idAnimal = @idAnimal;";

            try
            {
                Dictionary<string, object> values = new Dictionary<string, object>
                {
                    { "@idUsuario", avaliacao.IDUSUARIO },
                    { "@idAnimal", avaliacao.IDANIMAL }
                };

                QueryTableMySQL(delete, values);
            }
            catch
            {
                throw;
            }

        }
        #endregion

        #region SelectTable
        public DataTable SelectTableMySQL(string select, Dictionary<string, object> values)
        {
            try
            {

                USUARIO usuario = new USUARIO();
                DataTable dt = new DataTable();

                connectionMySQL.Open();
                transMySQL = connectionMySQL.BeginTransaction();

                MySqlCommand cmd = connectionMySQL.CreateCommand();

                cmd.CommandText = select;

                foreach (string key in values.Keys)
                {
                    cmd.Parameters.AddWithValue(key, values[key]);
                }

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    dt.Load(reader);
                }

                return dt;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }
        #endregion

        #region QueryTable
        public void QueryTableMySQL(string query, Dictionary<string, object> values)
        {
            try
            {
                connectionMySQL.Open();
                transMySQL = connectionMySQL.BeginTransaction();

                MySqlCommand cmd = connectionMySQL.CreateCommand();

                cmd.CommandText = query;

                foreach (string key in values.Keys)
                {
                    cmd.Parameters.AddWithValue(key, values[key]);
                }

                if (cmd.ExecuteNonQuery() > 0)
                    transMySQL.Commit();
                else
                    transMySQL.Rollback();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }
        #endregion

        #region CloseConnection
        public void CloseConnection()
        {
            connectionMySQL.Close();
        }
        #endregion

    }
}
