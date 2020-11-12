using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACME.Models
{
    public class Utility
    {
        public static int GetIdNumber(SqlConnection dbcon, string ctlkey)
        {
            string strquery = "SELECT idnumber FROM controltable" +
            " WHERE ctlkey = @ctlkey";
            SqlCommand myCmd = new SqlCommand(strquery, dbcon);
            myCmd.Parameters.AddWithValue("@ctlkey", SqlDbType.VarChar).Value = ctlkey;
            int count = Convert.ToInt32(myCmd.ExecuteScalar().ToString()) + 1;
            strquery = "UPDATE controltable SET idnumber = " + count +
            " where ctlkey = @ctlkey";
            myCmd = new SqlCommand(strquery, dbcon);
            myCmd.Parameters.AddWithValue("@ctlkey", SqlDbType.VarChar).Value = ctlkey;
            myCmd.ExecuteNonQuery();
            myCmd.Dispose();
            return count;
        }

        public static IEnumerable<SelectListItem>GetStatesDropDown(SqlConnection dbcon)
        {
            IList<SelectListItem> statelist = new List<SelectListItem>();
            SqlCommand cmd =
            new SqlCommand("select * from states order by 2", dbcon);
            SqlDataReader myReader;
            myReader = cmd.ExecuteReader();
            while (myReader.Read())
            {
                statelist.Add(new SelectListItem()
                {
                    Text = myReader["S_NAME"].ToString(),
                    Value = myReader["S_ABBREVIATION"].ToString()
                });
            }
            myReader.Close();
            cmd.Dispose();
            return statelist;
        }
    }
}