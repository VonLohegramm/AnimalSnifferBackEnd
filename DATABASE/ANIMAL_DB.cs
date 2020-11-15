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
        public void RecolherAnimal(ANIMAL animal)
        {
            string update = "update ANIMAL set ativo = @ativo where id = @id";

            try
            {
                Dictionary<string, object> values = new Dictionary<string, object>
                {
                    { "@id", animal.ID },
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
                    animal.IMAGEM = dt.Rows[0]["image"].ToString();
                    animal.LATITUDE = dt.Rows[0]["latitude"].ToString();
                    animal.LONGITUDE = dt.Rows[0]["longitude"].ToString();
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
                    animal.IMAGEM = row["imagem"].ToString();
                    animal.LATITUDE = row["latitude"].ToString();
                    animal.LONGITUDE = row["longitude"].ToString();
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
        }
        #endregion
    }
}
