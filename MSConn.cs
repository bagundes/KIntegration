using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace KIntegration
{
    public class MSConn : IDisposable
    {
        public SqlConnection Conn;

        public MSConn()
        {
            Connect();
        }

        public string ExecuteNonQuery(string sql)
        {
            var command = new SqlCommand(sql, Conn);
            command.ExecuteNonQuery();
            command.Dispose();

            var id = GetLastId();
            if (!String.IsNullOrEmpty(id))
                return id;
            else
                return null;
        }

        public SqlDataReader Reader(string sql)
        {
            var command = new SqlCommand(sql, Conn);
            return command.ExecuteReader();
        }

        private void Connect()
        {
            var stringConn = Properties.Resources.MSConn;
            Conn = new SqlConnection(stringConn);
            Conn.Open();
        }

        public string GetLastId()
        {
            string id = null;
            SqlCommand comando = new SqlCommand("SELECT SCOPE_IDENTITY()", Conn);
            SqlDataReader reader = comando.ExecuteReader();

            if (reader.Read())
                id = reader[0].ToString();

            reader.Close();
            return id;
        }

        public void Dispose()
        {
            Conn.Close();
        }
    }
}