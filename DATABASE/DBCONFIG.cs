using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace DATABASE
{
    public class DBCONFIG
    {
        #region Atributos
        private static string stringBancoMySQL = @"Server=localhost;Database=animalsniffer;Uid=root;Pwd=;";
        #endregion

        public static MySqlConnection OpenConnection()
        {
            return new MySqlConnection(stringBancoMySQL);
        }
    }
}
