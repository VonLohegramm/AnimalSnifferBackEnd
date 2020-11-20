using ENTIDADE;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DATABASE
{
    public class USUARIO_DB
    {
        #region Atributos
        private MySqlConnection connectionMySQL;
        private MySqlTransaction transMySQL;
        #endregion

        #region Constructor
        public USUARIO_DB()
        {
            connectionMySQL = DBCONFIG.OpenConnection();
        }
        #endregion

        #region InserirUsuario
        public void InserirUsuarioBancoDados(USUARIO usuario)
        {
            string insert = "insert into USUARIO (nome, cpf, senha, email) values (@nome, @cpf, @senha, @email);";

            try
            {
                Dictionary<string, object> values = new Dictionary<string, object>
                {
                    { "@nome",  usuario.NOME },
                    { "@cpf", usuario.CPF },
                    { "@email", usuario.EMAIL },
                    { "@senha", usuario.SENHA }
                };

                QueryTableMySQL(insert, values);
            }
            catch
            {
                throw;
            }

        }
        #endregion

        #region LoginUsuario
        public USUARIO SelectLoginBancoDados(USUARIO usuario)
        {
            string select = "select id, nome, cpf, email from USUARIO where email = @email and senha = @senha;";

            try
            {
                Dictionary<string, object> values = new Dictionary<string, object>
                {
                    { "@email",  usuario.EMAIL },
                    { "@senha", usuario.SENHA }
                };

                DataTable dt = SelectTableMySQL(select, values);

                if (dt.Rows.Count > 0)
                {
                    usuario.NOME = dt.Rows[0]["nome"].ToString();
                    usuario.CPF = dt.Rows[0]["cpf"].ToString();
                    usuario.EMAIL = dt.Rows[0]["email"].ToString();
                    usuario.ID = Convert.ToInt32(dt.Rows[0]["id"]);
                    usuario.SENHA = "";
                }

                return usuario;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region QueryTableMySQL
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

        #region SelectTableMySQL
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

        #region CloseConnection
        public void CloseConnection()
        {
            connectionMySQL.Close();
        }
        #endregion
    }
}
