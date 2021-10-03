using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    class Sqlclass
    {
        public static bool b = false;
        public static string sqlconkey = "Data Source=203.71.75.120,14333;Initial Catalog=English Exercise;User ID=yu;Password=s043126";
        public static SqlConnection sqlcon = new SqlConnection(sqlconkey);
        public static SqlDataAdapter sqla;
        public static DataTable totable(string a)//********
        {
            sqlcon.Open();
            DataTable dt = new DataTable();
            sqla = new SqlDataAdapter(a,sqlcon);
            sqla.Fill(dt);
            sqlcon.Close();
            return dt;
        }


    }
}
