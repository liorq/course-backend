using JwtWebApi.data;

using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NHibernate.Util;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Xml.Linq;
using JwtWebApi.tables;
using System.Security.Claims;
using courses.classes;
using JwtWebApi.classes;
using System.Net;
using System.Net.Http.Headers;

namespace JwtWebApi.Repositories
{
    public class StudentsRepository : IStudentsRepository
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        public StudentsRepository(DataContext context, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public async Task<User> CreateUser(CreateUserRequest user)
        {
            var newUser = new User
            {
                Username = user.Username,
                Password = user.Password,
                Role = user.Role,
                birthDate = user.BirthDate.ToString(),
                address = user.Address,
                phone = user.Phone,
                studentId = user.StudentId,
                isStudent = user.IsStudent,
                Name = user.Name,
                email= user.Username,
            };


            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
            return newUser;
        }
        public async Task<bool> RemoveUser(string userName)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userName);
            ////need to fix after 
            var userClassAttendees = await _context.ClassAttendees.FirstOrDefaultAsync(u => u.studentId == user.studentId);
            var userCourses = await _context.Courses.FirstOrDefaultAsync(u => u.CoursesId == user.studentId);

            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);


            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<User> GetUser(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            return user;
        }

        public async Task<List<User>> GetAllStudents()
        {
            var allStudents = await _context.Users.ToListAsync();
            return allStudents;
        }


        public async Task<List<ClassAttendees>> GetAllUsersArrivalTime()
        {

            var allStudentsAttendees = await _context.ClassAttendees.ToListAsync();
            return allStudentsAttendees;
        }
        public async Task<bool> UpdateUserArrivalTime(string userName, Courses course)
        {

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userName);

            string expectedArrivalTime = course?.date != null && course?.hours != null ? course.date + course.hours : null;

            var StudentsAttendees = await _context.ClassAttendees
                .FirstOrDefaultAsync(a => a.userName == userName&& a.CoursesId == course.CoursesId&&a.DateOfArrival== expectedArrivalTime);
         
   

            if (StudentsAttendees != null)
            {

                return false;
            }
        
            ClassAttendees currentAttendees = new()
            {
                name = user.Name,
                userName = userName,
                DateOfArrival = expectedArrivalTime,
                CoursesId = course.CoursesId,
                studentId= user.studentId,
                CoursesName=course.Name
            };

            _context.ClassAttendees.Add(currentAttendees);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ClassAttendees>> GetUserArrivalTime(string userName)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userName);
            var allStudentsAttendees = await _context.ClassAttendees.Where(a => a.studentId == user!.studentId).ToListAsync();
            return allStudentsAttendees;
        }
        public async Task<bool> ChangePassword(string userName, string newPassword, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userName && u.Password == password);
            if (user == null)
            {
                return false;
            }
            user.Password = newPassword;
            await _context.SaveChangesAsync();
            return true;

        }
        public async Task<bool> ChangeName(string userName, string newName, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userName && u.Password == password);
            if (user == null)
            {
                return false;
            }
            user.Name = newName;
            await _context.SaveChangesAsync();
            return true;

        }


        public async Task<bool> ChangeEmail(string userName, string newUserName, string password)
        {
     
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userName && u.Password == password);
          

            Console.WriteLine(user == null);

            Console.WriteLine(user);
            if (user == null)
            {
                return false;
            }
            user.email = newUserName;
            await _context.SaveChangesAsync();
            return true;

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