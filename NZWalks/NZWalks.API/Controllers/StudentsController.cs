using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWalks.API.Controllers
{
    // http://localhost:7160/api/students
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        // GET: http://localhost:7160/api/students
        [HttpGet]
        public IActionResult GetAllStudents()
        {
            var studentNames = new string[] { "John", "Jane", "Mark", "Emily", "David" };
            return Ok(studentNames); // 200 ok response with student names
        }
    }
}
