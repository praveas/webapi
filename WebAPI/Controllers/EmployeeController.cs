using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using WebAPI.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        // Dependency injections
        private readonly IConfiguration _configuration;

        // Dependency injection to get to Application path to folder
        private readonly IWebHostEnvironment _env;

        public EmployeeController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]

        public JsonResult Get()
        {
            string query = @"
                    select EmployeeId, EmployeeName, Department,
                    convert(varchar(10),DateOfJoining,120) as DateOfJoining,
                    PhotoFileName
                    from dbo.Employee";
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

        public JsonResult Post(Employee emp)
        {
            string query = @"
                    insert into dbo.Employee
                    (EmployeeName,Department,DateOfJoining,PhotoFileName)
                    values
                    (
                    '" + emp.EmployeeName + @"'
                    ,'" + emp.Department + @"'
                    ,'" + emp.DateOfJoing + @"'
                    ,'" + emp.PhotoFileName + @"'
                    )";

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

        // Put Method to Update Data into dbo.Employee table
        [HttpPut]

        public JsonResult Put(Employee emp)
        {
            string query = @"
                    update dbo.Employee set
                    EmployeeName = '" + emp.EmployeeName + @"'
                    ,Department = '" + emp.Department + @"'
                    ,DateOfJoining = '" + emp.DateOfJoing + @"'
                    ,PhotoFileName = '" + emp.PhotoFileName + @"'
                    where EmployeeId = " + emp.EmployeeId + @"
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
                    delete from dbo.Employee
                    where EmployeeId = '" + id + @"'
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

        // Creating API Method for Uploading Photos
        [Route("SaveFile")]
        [HttpPost]

        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + filename;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                return new JsonResult(filename);
            }
            catch (Exception)
            {
                return new JsonResult("anynomous.png");
            }
        }

        // Get all department names in Drop-Down Menu
        [Route("GetAllDepartmentNames")]

        public JsonResult GetAllDepartmentNames()
        {
            string query = @"
                    select DepartmentName from dbo.Department";

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
    }
}

