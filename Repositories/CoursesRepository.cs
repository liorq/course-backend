using JwtWebApi.tables;
using Microsoft.EntityFrameworkCore;
using JwtWebApi.data;
//using NHibernate.Util;
//using System.Linq;
//using System.Xml.Linq;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using courses.classes;

namespace JwtWebApi.Repositories
{
    public class CoursesRepository : ICoursesRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CoursesRepository(IHttpContextAccessor httpContextAccessor, DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }



        public async Task<User> GetUser(string username)
        {
            return  await _context.Users.FirstOrDefaultAsync(user => user.Username == username);
        }




        public async Task<List<Courses>> GetAllCourses()
        {
            var courses = await _context.Courses.Where(c=>c.StudentId== "admin").ToListAsync();
            return courses;
        }
        public async Task<List<Courses>> GetUserCourses(string userName)
        {
            var courses = await _context.Courses.Where(c => c.StudentId == userName).ToListAsync();
            return courses;
  
        }
        public async Task<bool> RemoveCourse(string CoursesId)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.CoursesId == CoursesId);
            if (course == null)
            {
                return false;
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<bool> AddNewCourse(Courses course)
        {
            var allCourses=await _context.Courses.ToListAsync();
            var courseName = allCourses.FirstOrDefault(c => c.CoursesId == course.CoursesId);
            if (courseName != null)
            {
                return false;
            }
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Courses> BuyCourse(Courses course)
        {
            var allCourses = await _context.Courses.ToListAsync();
            var courseName = allCourses.FirstOrDefault(c => c.StudentId == course.StudentId&&c.CoursesId == course.CoursesId);
       

            if (courseName != null)
            {
                return null;
            }
            course.Id = null;
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return course;
        }



        public string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.Username));
            claims.Add(new Claim(ClaimTypes.Role, user.Role));

            if (user.Role == "ROLE_OWNER")
            {
                claims.Add(new Claim(ClaimTypes.Role, "ROLE_MANAGER"));
            }
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(2),
                signingCredentials: creds
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
        public string? getUserNameByToken()
        {
            var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (token == "")
                return "you dont have token send";

            var handler = new JwtSecurityTokenHandler();
            var decodedToken = handler.ReadJwtToken(token);
            return decodedToken?.Claims?.ToArray()[0]?.Value;
        }
    }
}
