using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace GESTION_DE_PROJET.Shared
{
    class Database
    {
        public static  SqlConnection cnx = new SqlConnection();
        public SqlCommand cmd = new SqlCommand();
        //public SqlDataReader dr = new SqlDataReader();
        public SqlDataAdapter dt = new SqlDataAdapter();

        public static  void connecter()
        {
            if(cnx is null)
                 cnx = new SqlConnection("Data Source=.;Initial Catalog=gestionprojet;Integrated Security=True");
            if (cnx.State == System.Data.ConnectionState.Closed || cnx.State == System.Data.ConnectionState.Broken)
                cnx.Open();
        }

        public static  void deconnecter()
        {
            if (cnx.State == System.Data.ConnectionState.Open)
                cnx.Close();
        }

        public static DataTable GetdDataFromDatabase(string sql)
        {
            try
            {
                connecter();

                SqlCommand cmd = new SqlCommand(sql, cnx);
                var data = new DataTable();
                data.Load(cmd.ExecuteReader());
                return data;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        public static int  UpdateDatabase(string Sql)
        {
            try
            {
                connecter();

                SqlCommand cmd = new SqlCommand(Sql, cnx);
                int x = cmd.ExecuteNonQuery();
                return x;
            }
            catch(Exception ex)
            {
                return -1;
            }
          
        }

        public static string  GetOneRow(string Sql)
        {
             try
            {
                connecter();

                SqlCommand cmd = new SqlCommand(Sql, cnx);
               object x = cmd.ExecuteScalar();
                return x.ToString();
            }
            catch(Exception ex)
            {
                return "";
            }
        }
    }
  

}
