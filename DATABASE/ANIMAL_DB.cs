using ENTIDADE;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DATABASE
{
    public class ANIMAL_DB
    {
        #region Atributos
        private MySqlConnection connectionMySQL;
        private MySqlTransaction transMySQL;
        #endregion

        #region Constructor
        public ANIMAL_DB()
        {
            connectionMySQL = DBCONFIG.OpenConnection();
        }
        #endregion

        #region InserirAnimal
        public void InserirAnimal(ANIMAL animal)
        {
            string insert = "insert into ANIMAL(tipo, raca, sexo, descricao, latitude, longitude, imagem, idusuario) values(@tipo, @raca, @sexo, @descricao, @latitude, @longitude, @imagem, @idusuario);";

            try
            {
                Dictionary<string, object> values = new Dictionary<string, object>
                {
                    { "@tipo", animal.TIPO },
                    { "@raca", animal.RACA },
                    { "@sexo", animal.SEXO },
                    { "@latitude", animal.LATITUDE },
                    { "@longitude", animal.LONGITUDE },
                    { "@imagem", animal.IMAGEM },
                    { "@descricao",  animal.DESCRICAO },
                    { "@ativo", true },
                    { "@idusuario", animal.IDUSUARIO }
                };

                QueryTableMySQL(insert, values);
            }
            catch
            {
                throw;
            }

        }
        #endregion

        #region RecolherUsuario
        public void RecolherAnimal(int id)
        {
            string update = "update ANIMAL set ativo = @ativo where id = @id";

            try
            {
                Dictionary<string, object> values = new Dictionary<string, object>
                {
                    { "@id", id },
                    { "@ativo", false }
                };

                QueryTableMySQL(update, values);
            }
            catch
            {
                throw;
            }

        }
        #endregion

        #region SelecionarAnimal
        public ANIMAL SelecionarAnimal (int id)
        {
            string select = "select * from ANIMAL where id = @id";
            ANIMAL animal = new ANIMAL();

            try
            {
                Dictionary<string, object> values = new Dictionary<string, object>
                {
                    { "@id",  id }
                };

                DataTable dt = SelectTableMySQL(select, values);

                if (dt.Rows.Count > 0)
                {
                    animal.ID = Convert.ToInt32(dt.Rows[0]["id"].ToString());
                    animal.RACA = dt.Rows[0]["raca"].ToString();
                    animal.TIPO = dt.Rows[0]["tipo"].ToString();
                    animal.DESCRICAO = dt.Rows[0]["descricao"].ToString();
                    byte[] an = (byte[])dt.Rows[0]["imagem"];
                    animal.IMAGEM = System.Text.Encoding.UTF8.GetString(an);
                    animal.LATITUDE = Convert.ToDecimal(dt.Rows[0]["latitude"]);
                    animal.LONGITUDE = Convert.ToDecimal(dt.Rows[0]["longitude"]);
                    animal.SEXO = dt.Rows[0]["sexo"].ToString();
                    animal.IDUSUARIO = Convert.ToInt32(dt.Rows[0]["idusuario"].ToString());
                }

                return animal;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region SelecionarAnimaisAtivos
        public List<ANIMAL> SelecionarAnimaisAtivos()
        {
            string select = "select * from ANIMAL where ativo = @ativo";

            List<ANIMAL> animais = new List<ANIMAL>();
            ANIMAL animal = new ANIMAL();

            try
            {
                Dictionary<string, object> values = new Dictionary<string, object>
                {
                    { "@ativo",  true }
                };

                DataTable dt = SelectTableMySQL(select, values);

                foreach(DataRow row in dt.Rows)
                {
                    animal.ID = Convert.ToInt32(row["id"].ToString());
                    animal.RACA = row["raca"].ToString();
                    animal.TIPO = row["tipo"].ToString();
                    animal.DESCRICAO = row["descricao"].ToString();
                    byte[] an = (byte[])row["imagem"];
                    animal.IMAGEM = System.Text.Encoding.UTF8.GetString(an);
                    animal.LATITUDE = Convert.ToDecimal(row["latitude"]);
                    animal.LONGITUDE = Convert.ToDecimal(row["longitude"]);
                    animal.SEXO = row["sexo"].ToString();
                    animal.IDUSUARIO = Convert.ToInt32(dt.Rows[0]["idusuario"]);

                    animais.Add(animal);
                    animal = new ANIMAL();
                }

                return animais;
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
