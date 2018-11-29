using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvenienceStore
{
    class SqlManager
    {
        public static SqlManager sqlManager = new SqlManager();
        private MySqlConnection conn;

        private const string serverConfig = "server=localhost;user=root;database=mycontacts;port=3306;password=123";

        private SqlManager() {
            conn = new MySqlConnection(serverConfig);
            connectDB();
        }

        public SqlManager getManager()
        {
            return sqlManager;
        }

        public string query(string sql)
        {
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            StringBuilder sb = new StringBuilder();

            while (rdr.Read())
            {
                sb.AppendLine(rdr[1].ToString());
            }

            return sb.ToString();
        }

        public void connectDB()
        {
            conn.Open();
        }

        public void disconnectDB()
        {
            conn.Close();
        }
    }
}
