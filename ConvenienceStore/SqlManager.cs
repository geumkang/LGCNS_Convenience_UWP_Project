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
        public static SqlManager sqlManager;
        private static MySqlConnection conn;

        private const string serverConfig = "server=localhost;user=root;database=cns;port=3306;password=123";

        private SqlManager() {
            conn = new MySqlConnection(serverConfig);
        }

        public static SqlManager getManager()
        {
            return sqlManager;
        }

        public static string query(string sql, int columnNum)
        {
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            StringBuilder sb = new StringBuilder();

            while (rdr.Read())
            {
                for (int i = 0; i < columnNum; i++)
                {
                    sb.Append(rdr[i].ToString());
                    sb.Append("\t");
                }
                sb.Append("\n");
            }

            rdr.Dispose();
            return sb.ToString();
        }

        public static void insertQuery(string sql)
        {
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }

        public static void connectDB()
        {
            sqlManager = new SqlManager();
            conn.Open();
        }

        public static void disconnectDB()
        {
            conn.Close();
        }
    }
}
