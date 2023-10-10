using FirstApi_AdoNet.DAL;
using FirstApi_AdoNet.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;
using System.Data;

namespace FirstApi_AdoNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly StudentDAL studentdal;

        public StudentsController(StudentDAL studentdal)
        {
            this.studentdal = studentdal;
        }

        [HttpGet("GetAllStudents")]
        public IActionResult GetAllStudents()
        {
            try
            {
                NameValueCollection parameters = new NameValueCollection();
                DataTable data = studentdal.GetData("GetAllStudent", parameters);

                // Check if data is retrieved successfully
                if (data != null && data.Rows.Count > 0)
                {
                    // Convert DataTable to a list of Student objects
                    List<Student> studentList = new List<Student>();
                    foreach (DataRow row in data.Rows)
                    {
                        Student student = new Student
                        {
                            Sid = Convert.ToInt32(row["Sid"]),
                            Sname = row["Sname"].ToString()
                        };
                        studentList.Add(student);
                    }

                    return Ok(studentList);
                }
                else
                {
                    return NotFound("No students found.");
                }
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpGet("GetStudentById/{id}")]
        public IActionResult GetStudentById(int id)
        {
            NameValueCollection parameters = new NameValueCollection();
            DataTable data = studentdal.GetStudentById("GetStudentById", parameters, id);

            if (data != null && data.Rows.Count > 0)
            {
                DataRow row = data.Rows[0];
                Student student = new Student
                {
                    Sid = Convert.ToInt32(row["Sid"]),
                    Sname = row["Sname"].ToString()
                };
                return Ok(student);
            }
            else
            {
                return NotFound("Student not found.");
            }
        }


        [HttpPost("CreateStudent")]
        public IActionResult CreateStudent([FromBody] string Sname)
        {
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("@Sname", Sname);
            DataTable data = studentdal.CreateStudent("CreateStudent", parameters, Sname);

            return Ok("Dats Save Done.");
            
        }

    }
}
