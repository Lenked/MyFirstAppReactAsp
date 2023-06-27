using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MyWebAppTutorial.Models;
using System.Data;
using System.IO;

namespace MyWebAppTutorial.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public EmployeeController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            try
            {
                string query = @"
                            select EmployeeId,EmployeeName, Department,
                            convert(varchar(10),DateOfJoining,120) as DateOfJoining, PhotoFilename
                            from 
                            dbo.Employee
                            ";
                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
                SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                        myCon.Close();
                    }
                }
                return new JsonResult(table);
            }
            catch (System.Exception e)
            {
                return new JsonResult(e.Message);
            }
        }

        [HttpPost]
        public JsonResult Post(Employee employee)
        {
            try
            {
                string query = @"
                            INSERT INTO [dbo].[Employee](EmployeeName, Department, DateOfJoining,PhotoFilename) 
                               VALUES (@EmployeeName, @Department, @DateOfJoining, @PhotoFilename)
                            ";
                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
                SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
                        myCommand.Parameters.AddWithValue("@Department", employee.Department);
                        myCommand.Parameters.AddWithValue("@DateOfJoining", employee.DateOfJoining);
                        myCommand.Parameters.AddWithValue("@PhotoFilename", employee.PhotoFilename);
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                        myCon.Close();
                    }
                }
                return new JsonResult("Ajouter avec succes!");
            }
            catch (System.Exception e)
            {
                return new JsonResult(e.Message);
            }
        }

        [HttpPut]
        public JsonResult Put(Employee employee)
        {
            try
            {
                string query = @"
                           UPDATE [dbo].[Employee] SET EmployeeName=@EmployeeName, Department=@Department, DateOfJoining=@DateOfJoining,PhotoFilename=@PhotoFilename 
                               WHERE EmployeeId=@EmployeeId
                            ";
                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
                SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
                        myCommand.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
                        myCommand.Parameters.AddWithValue("@Department", employee.Department);
                        myCommand.Parameters.AddWithValue("@DateOfJoining", employee.DateOfJoining);
                        myCommand.Parameters.AddWithValue("@PhotoFilename", employee.PhotoFilename);
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                        myCon.Close();
                    }
                }
                return new JsonResult("Modification effectué avec succes!");
            }
            catch (System.Exception e)
            {
                return new JsonResult(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            try
            {
                string query = @"
                            DELETE FROM [dbo].[Employee] WHERE EmployeeId=@EmployeeId
                            ";
                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
                SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@EmployeeId", id);
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                        myCon.Close();
                    }
                }
                return new JsonResult("Supprimer avec succes!");
            }
            catch (System.Exception e)
            {
                return new JsonResult(e.Message);
            }
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string fileneame = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + fileneame;

                using(var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                return new JsonResult(fileneame);    
            }
            catch (System.Exception)
            {
                return new JsonResult("anonymous.png");
            }
        }

    }
}
