using courses.classes;

using JwtWebApi.Repositories;
using JwtWebApi.tables;
using Microsoft.AspNetCore.Mvc;

namespace JwtWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        public static User user = new User();
        private readonly IStudentsRepository _studentsRepository;

        public StudentsController( IStudentsRepository studentsRepository)
        {
            _studentsRepository = studentsRepository;
        }



        [HttpGet("users/{user}/attendees")]
        public async Task<IActionResult> getAllUserAttendees()
        {
            string userName = _studentsRepository.getUserNameByToken();
            var res = await _studentsRepository.GetUserArrivalTime(userName);

            if (res?.Count > 0)
            {
                return Ok(res);
            }
            return BadRequest("");

        }
        [HttpGet("")]
        public async Task<IActionResult> getAllUsers()
        {
            string userName = _studentsRepository.getUserNameByToken();
            var res = await _studentsRepository.GetAllStudents();

            if (res?.Count > 0)
            {
                return Ok(res);
            }
            return BadRequest("");

        }


        [HttpGet("users/attendees")]
        public async Task<IActionResult> getAllAttendees()
        {
            var userId = _studentsRepository.getUserNameByToken();
            if (userId != null)
            {
                var res = await _studentsRepository.GetAllUsersArrivalTime();

                if (res != null && userId != null)
                    return Ok(res);
            }

            return BadRequest("");

        }

        
        [HttpPost("users/{userId}/{courseName}/arrival-time")]
        public async Task<IActionResult> addAttendees([FromBody] Courses course)
        {
            var userId = _studentsRepository.getUserNameByToken();
            if (userId != null)
            {
                Console.WriteLine(course.date);
                var res = await _studentsRepository.UpdateUserArrivalTime(userId, course);

                if (res != false)
                    return Ok(res);
            }

            return BadRequest("");

        }

        
    }
}

