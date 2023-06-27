using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MyWebAppTutorial.Models;
using System.Data;
using System.Reflection.Metadata.Ecma335;

namespace MyWebAppTutorial.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DepartmentController : Controller
    {
        private readonly IConfiguration _configuration;
        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            try
            {
                string query = @"
                            select DepartmentId, DepartmentName from 
                            dbo.Department
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
        public JsonResult Post(Department dep)
        {
            try
            {
                string query = @"
                            INSERT INTO [dbo].[Department]([DepartmentName]) VALUES (@DepartmentName)
                            ";
                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
                SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@DepartmentName", dep.DepartmentName);
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
        public JsonResult Put(Department dep)
        {
            try
            {
                string query = @"
                            UPDATE [dbo].[Department] SET DepartmentName = @DepartmentName WHERE DepartmentId=@DepartmentId
                            ";
                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
                SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@DepartmentId", dep.DepartmentId);
                        myCommand.Parameters.AddWithValue("@DepartmentName", dep.DepartmentName);
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
                            DELETE FROM [dbo].[Department] WHERE DepartmentId=@DepartmentId
                            ";
                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
                SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@DepartmentId", id);
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

    }
}
