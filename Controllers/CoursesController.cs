
using JwtWebApi.Repositories;
using JwtWebApi.tables;
using Microsoft.AspNetCore.Mvc;
using courses.classes;

namespace JwtWebApi.Controllers
{
    [ApiController]
    public class CoursesController : ControllerBase
    {
        public static User user = new User();
        private readonly ICoursesRepository _coursesRepository;

        public CoursesController( ICoursesRepository CoursesRepository)
        {
 
            _coursesRepository = CoursesRepository;

        }

 

        [HttpGet("")]
        public async Task<IActionResult> getAllCourses()
        {

            var res = await _coursesRepository.GetAllCourses();

            if (res?.Count > 0)
            {
                return Ok(res);
            }
            return BadRequest("");

        }


        [HttpGet("users/{user}/courses")]
       
        public async Task<IActionResult> getAllUserCourses()
        {
            var userId = _coursesRepository.getUserNameByToken();
            Console.WriteLine(userId);
            if (userId != null){
              var res = await _coursesRepository.GetUserCourses(userId);

                if (res != null&& userId!=null)
                return Ok(res);
            }
          
            return BadRequest("");

        }

        [HttpPost("courses/{courseName}/buy")]
        public async Task<IActionResult> BuyCourse(Courses course)
        {
            var userId = _coursesRepository.getUserNameByToken();
            course.StudentId = userId;
            //Console.WriteLine(userId);
            var CoursesAdded = await _coursesRepository.BuyCourse(course);
            if (CoursesAdded!=null)
                return Ok(CoursesAdded);
            else
                return BadRequest("Courses already exicted");
        }



        [HttpPost("courses/{courseName}")]
        public async Task<IActionResult> AddNewCourse([FromBody] Courses course)
        {
            /// //new course for everyone

            var userId =  _coursesRepository.getUserNameByToken();

            var isCoursesAdded = await _coursesRepository.AddNewCourse(course);
            if (isCoursesAdded)
                return Ok("Courses added");
            else
                return BadRequest("Courses already exicted");
        }

        [HttpDelete("courses/{CoursesId}")]
        public async Task<IActionResult> isPossibleRemoveCourse(string CoursesId)
        {
            /// //new course for everyone

            var userId = _coursesRepository.getUserNameByToken();
            var isCoursesRemoved = await _coursesRepository.RemoveCourse(CoursesId);
            if (isCoursesRemoved)
                return Ok("Courses removed");
            else
                return BadRequest("failed course removal");
        }
    }
}
