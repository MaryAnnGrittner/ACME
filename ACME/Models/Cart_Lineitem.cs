using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ACME.Models
{
    public class Cart_Lineitem
    {
        [Required, Key, Column(Order = 1)]
        public int CartNumber { get; set; }
        [Required, Key, Column(Order = 2), MaxLength(10)]
        public string ProductId { get; set; }
        [Required, Range(minimum: 1, maximum: 99, ErrorMessage = "Qty sb 1-99")]
        public int Quantity { get; set; }

        public static Int32 CartUpSert(SqlConnection dbcon, Cart_Lineitem cart)
        {
            SqlCommand cmd = new SqlCommand("sp_cart_upsert", dbcon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@cartid", SqlDbType.Int).Value = cart.CartNumber;
            cmd.Parameters.Add("@prodid", SqlDbType.VarChar).Value = cart.ProductId;
            cmd.Parameters.Add("@qty", SqlDbType.Int).Value = cart.Quantity;
            int intCnt = cmd.ExecuteNonQuery();
            return intCnt;
        }
        public static int CUDCartLineitem(SqlConnection dbcon, string CUDAction, Cart_Lineitem obj)
        {
            SqlCommand cmd = new SqlCommand();
            if (CUDAction == "update")
            {
                cmd.CommandText = "update cart_lineitems set Quantity = @Quantity " +
                "Where CartNumber = @CartNumber and ProductId = @ProductId";
                cmd.Parameters.AddWithValue("@CartNumber", SqlDbType.Int).Value = obj.CartNumber;
                cmd.Parameters.AddWithValue("@ProductId", SqlDbType.NVarChar).Value = obj.ProductId;
                cmd.Parameters.AddWithValue("@Quantity", SqlDbType.Int).Value = obj.Quantity;

            }
            else if (CUDAction == "delete")
            {
                cmd.CommandText = "delete cart_lineitems Where CartNumber = @CartNumber and ProductId = @ProductId";
                cmd.Parameters.AddWithValue("@CartNumber", SqlDbType.Int).Value = obj.CartNumber;
                cmd.Parameters.AddWithValue("@ProductId", SqlDbType.NVarChar).Value = obj.ProductId;
            }
            cmd.Connection = dbcon;
            int intResult = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return intResult;
        }
    }
}