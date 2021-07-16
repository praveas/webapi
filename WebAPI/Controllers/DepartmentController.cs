using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using WebAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]

        public JsonResult Get()
        {
            string query = @"
                    select DepartmentId, DepartmentName from dbo.Department";
            DataTable table = new DataTable(); //need to call System.Data
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpPost]

        public JsonResult Post(Department dep)
        {
            string query = @"
                    insert into dbo.Department values
                    ('" + dep.DepartmentName + @"')";

            DataTable table = new DataTable(); //need to call System.Data
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;

                    myReader.Close();
                    myCon.Close();
                }

            }
            return new JsonResult("Added Successfully");
        }

        // Put Method to Update Data into dbo.Department table
        [HttpPut]

        public JsonResult Put(Department dep)
        {
            string query = @"
                    update dbo.Department set
                    DepartmentName = '" + dep.DepartmentName + @"'
                    where DepartmentId = " + dep.DepartmentId + @"
                    ";

            DataTable table = new DataTable(); //need to call System.Data
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;

                    myReader.Close();
                    myCon.Close();
                }

            }
            return new JsonResult("Update Successfully");
        }

        // Delete Method
        // since we are sending id in URL, we need to add it in root parameter
        [HttpDelete("{id}")]

        public JsonResult Delete(int id)
        {
            string query = @"
                    delete from dbo.Department
                    where DepartmentId = '"+ id + @"'
                    ";

            DataTable table = new DataTable(); //need to call System.Data
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;

                    myReader.Close();
                    myCon.Close();
                }

            }
            return new JsonResult("Delete Successfully");
        }
    }
}
