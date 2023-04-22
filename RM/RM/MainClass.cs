using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM
{
    internal class MainClass
    {
        public static readonly string  con_string = "Data Source=LAPTOP-AHHL9HD4;Initial Catalog=RM;Integrated Security=True;";

        public static SqlConnection con = new SqlConnection(con_string);

        public static bool IsValidUser(string user, string pass)
        {
            bool IsValid = false;
            string query = @"SELECT * FROM users WHERE username = '" + user + "' AND upass = '" + pass + "'";
            using (SqlConnection con = new SqlConnection(con_string))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand(query, con);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    IsValid = true;
                }
            }
            return IsValid;
        }
    }
}
