using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Mvc_customers.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc_customers.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IConfiguration _configuration;

        public CustomerController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public IActionResult Index()
        {
            DataTable da = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlDataAdapter sdd = new SqlDataAdapter("Select *from customers", sqlConnection);
                sdd.Fill(da);
            }
            return View(da);
        }
        [HttpGet]

        public IActionResult AddOrEdit(int? id)
        {
            Customers customers = new Customers();
            if(id>0)
            {
                 customers = FetchById(id);
            }
            return View(customers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEdit(Customers customers,int CustomerID)
        {
            if(ModelState.IsValid)
            {
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
                {
                    sqlConnection.Open();

                    if(CustomerID == 0)
                    {
                        string query = "Insert into Customers Values(@Firstname,@Middlename,@Lastname,@Address)";
                        SqlCommand sqlCmd = new SqlCommand(query, sqlConnection);
                        sqlCmd.Parameters.AddWithValue("@Firstname", customers.Firstname);
                        sqlCmd.Parameters.AddWithValue("@Middlename", customers.Middlename);
                        sqlCmd.Parameters.AddWithValue("@Lastname", customers.Lastname);
                        sqlCmd.Parameters.AddWithValue("@Address", customers.Address);
                        sqlCmd.ExecuteNonQuery();
                    }
                    else
                    {
                        string query = "Update Customers SET Firstname=@Firstname,Middlename=@Middlename,Lastname=@Lastname,Address=@Address where CustomerID=@CustomerID";
                        SqlCommand sqlcmd = new SqlCommand(query, sqlConnection);
                        sqlcmd.Parameters.AddWithValue("@CustomerID", customers.CustomerID);
                        sqlcmd.Parameters.AddWithValue("@Firstname", customers.Firstname);
                        sqlcmd.Parameters.AddWithValue("@Middlename", customers.Middlename);
                        sqlcmd.Parameters.AddWithValue("@Lastname", customers.Lastname);
                        sqlcmd.Parameters.AddWithValue("@Address", customers.Address);
                        sqlcmd.ExecuteNonQuery();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customers);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Customers customers = FetchById(id);

            return View(customers);
        }

        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        
        public IActionResult DeleteConfirmed(int? id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                string query = "Delete from customers where CustomerID = @CustomerID";
                SqlCommand sdd = new SqlCommand(query, sqlConnection);
                sdd.Parameters.AddWithValue("@CustomerID", id);
                sdd.ExecuteNonQuery();
            }
            return RedirectToAction(nameof(Index));
        }
        [NonAction]
        public Customers FetchById(int? id)
        {
            Customers customer = new Customers();
            DataTable da = new DataTable();

            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                string query = "Select *from customers where CustomerID=@CustomerID";
                SqlDataAdapter sdd = new SqlDataAdapter(query, sqlConnection);
                sdd.SelectCommand.Parameters.AddWithValue("@CustomerID", id);
                sdd.Fill(da);

                if(da.Rows.Count==1)
                {
                    customer.CustomerID = Convert.ToInt32(da.Rows[0]["CustomerID"].ToString());
                    customer.Firstname = da.Rows[0]["Firstname"].ToString();
                    customer.Middlename = da.Rows[0]["Middlename"].ToString();
                    customer.Lastname = da.Rows[0]["Lastname"].ToString();
                    customer.Address = da.Rows[0]["Address"].ToString();
                }

                return customer;
            }
            
        }

    }
}
