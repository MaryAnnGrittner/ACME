using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ACME.Models
{
    public class Customer
    {
        [Required, Key]
        public int CustNumber { get; set; }
        public string Email { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public string PWD { get; set; }

        public static Customer GetCustomerSingle(SqlConnection dbcon, int custid, string email)
        {
            Customer obj = new Customer();
            SqlCommand cmd;
            if (custid > 0)
            {
                cmd = new SqlCommand("select * from customers where custnumber = @id", dbcon);
                cmd.Parameters.AddWithValue("@id", SqlDbType.Int).Value = custid;
            }
            else
            {
                cmd = new SqlCommand("select * from customers where email = @email", dbcon);
                cmd.Parameters.AddWithValue("@email", SqlDbType.VarChar).Value = email;
            }
            SqlDataReader myReader;
            myReader = cmd.ExecuteReader();
            while (myReader.Read())
            {
                obj.CustNumber = Convert.ToInt32(myReader["Custnumber"].ToString());
                obj.Email = myReader["email"].ToString();
                obj.LastName = myReader["lastname"].ToString();
                obj.FirstName = myReader["firstname"].ToString();
                obj.Address = myReader["address"].ToString();
                obj.City = myReader["city"].ToString();
                obj.State = myReader["state"].ToString();
                obj.ZipCode = myReader["zipcode"].ToString();
                obj.PhoneNumber = myReader["phonenumber"].ToString();
                obj.PWD = myReader["pwd"].ToString();
            }
            myReader.Close();
            return obj;
        }

    }
}