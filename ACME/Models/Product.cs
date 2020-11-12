using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace ACME.Models
{
    public class Product
    {
        [Required, Key, MaxLength(10)]
        public string ProductID { get; set; }
        [Required, MaxLength(20)]
        public string Name { get; set; }
        [Required, MaxLength(20)]
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string CategoryID { get; set; }
        public string ImageFile { get; set; }
        public decimal UnitPrice { get; set; }
        public int OnHand { get; set; }

        public static Product GetProductSingle(SqlConnection dbcon, string id)
        {
            Product obj = new Product();
            string strsql = "select * from products where productid = '" + id + "'";
            SqlCommand cmd = new SqlCommand(strsql, dbcon);
            SqlDataReader myReader;
            myReader = cmd.ExecuteReader();
            while (myReader.Read())
            {
                //put object data property assignment statements here
                obj.ProductID = myReader["ProductID"].ToString();
                obj.Name = myReader["Name"].ToString();
                obj.ShortDescription = myReader["ShortDescription"].ToString();
                obj.LongDescription = myReader["LongDescription"].ToString();
                obj.CategoryID = myReader["CategoryID"].ToString();
                obj.ImageFile = myReader["ImageFile"].ToString();
                obj.UnitPrice = Convert.ToDecimal(myReader["UnitPrice"].ToString());
                obj.OnHand = Convert.ToInt32(myReader["OnHand"].ToString());
            }
            myReader.Close();
            return obj;
        }
        public static List<Product> GetProductList(SqlConnection dbcon, string SqlClause)
        {
            List<Product> itemlist = new List<Product>();
            string strsql = "select * from products " + SqlClause;
            SqlCommand cmd = new SqlCommand(strsql, dbcon);
            SqlDataReader myReader;
            myReader = cmd.ExecuteReader();
            while (myReader.Read())
            {
                Product obj = new Product();
                obj.ProductID = myReader["ProductID"].ToString();
                obj.Name = myReader["Name"].ToString();
                obj.ShortDescription = myReader["ShortDescription"].ToString();
                obj.LongDescription = myReader["LongDescription"].ToString();
                obj.CategoryID = myReader["CategoryID"].ToString();
                obj.ImageFile = myReader["ImageFile"].ToString();
                obj.UnitPrice = Convert.ToDecimal(myReader["UnitPrice"].ToString());
                obj.OnHand = Convert.ToInt32(myReader["OnHand"].ToString());
                itemlist.Add(obj);
            }
            myReader.Close();
            return itemlist;
        }
        public static int CUDProduct(SqlConnection dbcon, string CUDAction, Product obj)
        {
            SqlCommand cmd = new SqlCommand();
            if (CUDAction == "create")
            {
                cmd.CommandText = "insert into products " +
                "Values (@ProductID, @Name, @ShortDescription, @LongDescription, @CategoryID, @ImageFile, @UnitPrice, @OnHand)";
                //copy parameter assignment statements here
                cmd.Parameters.AddWithValue("@ProductID", SqlDbType.VarChar).Value = obj.ProductID;
                cmd.Parameters.AddWithValue("@Name", SqlDbType.VarChar).Value = obj.Name;
                cmd.Parameters.AddWithValue("@ShortDescription", SqlDbType.VarChar).Value = obj.ShortDescription;
                cmd.Parameters.AddWithValue("@LongDescription", SqlDbType.VarChar).Value = obj.LongDescription;
                cmd.Parameters.AddWithValue("@CategoryID", SqlDbType.VarChar).Value = obj.CategoryID;
                cmd.Parameters.AddWithValue("@ImageFile", SqlDbType.VarChar).Value = obj.ImageFile;
                cmd.Parameters.AddWithValue("@UnitPrice", SqlDbType.Decimal).Value = obj.UnitPrice;
                cmd.Parameters.AddWithValue("@OnHand", SqlDbType.Int).Value = obj.OnHand;
            }
            else if (CUDAction == "update")
            {
                cmd.CommandText = "update products set Name = @Name, ShortDescription = @ShortDescription, LongDescription = @LongDescription, " +
                "CategoryID = @CategoryID, ImageFile = @ImageFile, UnitPrice = @UnitPrice, OnHand = @OnHand " +
                "Where productid = @ProductID";
                //copy parameter assignment statements here
                cmd.Parameters.AddWithValue("@ProductID", SqlDbType.VarChar).Value = obj.ProductID;
                cmd.Parameters.AddWithValue("@Name", SqlDbType.VarChar).Value = obj.Name;
                cmd.Parameters.AddWithValue("@ShortDescription", SqlDbType.VarChar).Value = obj.ShortDescription;
                cmd.Parameters.AddWithValue("@LongDescription", SqlDbType.VarChar).Value = obj.LongDescription;
                cmd.Parameters.AddWithValue("@CategoryID", SqlDbType.VarChar).Value = obj.CategoryID;
                cmd.Parameters.AddWithValue("@ImageFile", SqlDbType.VarChar).Value = obj.ImageFile;
                cmd.Parameters.AddWithValue("@UnitPrice", SqlDbType.Decimal).Value = obj.UnitPrice;
                cmd.Parameters.AddWithValue("@OnHand", SqlDbType.Int).Value = obj.OnHand;
            }
            else if (CUDAction == "delete")
            {
                cmd.CommandText = "delete products where productid = @ProductID";
                cmd.Parameters.AddWithValue("@ProductID", SqlDbType.VarChar).Value = obj.ProductID;
            }
            cmd.Connection = dbcon;
            int intResult = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return intResult;
        }
    }
}