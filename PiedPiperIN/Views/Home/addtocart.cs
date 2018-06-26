using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using PiedPiperIN.Models;
namespace PiedPiperIN.Views.Home
{
    public class addtocart
    {
        static void main(String[] strgs)
        {
            string connectionString = "";
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlCommand sqlCmd = new SqlCommand("addtocart", sqlCon);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                
               
                sqlCmd.ExecuteNonQuery();

                sqlCon.Close();

                using (PiedPiperINEntities cartdb = new PiedPiperINEntities())
                {

                }
            }
        }
    }
}