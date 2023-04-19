using JwtWebApi.data;
using JwtWebApi.Repositories;
using JwtWebApi.tables;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Xml;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using JwtWebApi.classes;

namespace JwtWebApi.Controllers
{
    [ApiController]

    public class AccountController : ControllerBase
    {
        public static User user = new User();
        private readonly DataContext _context;
        //private readonly IConfiguration _configuration;
        //private readonly ICoursesRepository _heroesRepository;
        //private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IStudentsRepository _userRepository;



        public AccountController( DataContext context, IStudentsRepository userRepository)
        {
            //_httpContextAccessor = httpContextAccessor;
            //_configuration = configuration;
            _context = context;
            //_heroesRepository = heroesRepository;
            _userRepository = userRepository;
           
        }

        [HttpPost("/signUp")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
           
           

            var existingUser = await _context.Users.FirstOrDefaultAsync(user => user.Username == request.Username);
            Console.WriteLine(existingUser);
            if (existingUser != null)
                return BadRequest("Username already exists");
            else
            {
                await _userRepository.CreateUser(request);
                return Ok("UserCreatedSuccessfully");
            }
        }
        [HttpPost("/getUserType")]
        public async Task<IActionResult> getUserType([FromBody] ReqData reqData)
        {
            var user = await _userRepository.GetUser(reqData.username);
            if (user == null || user.Username != reqData.username)
                return BadRequest("User Not Found");

            var typeOfUser = (bool)user.isStudent ? "student" : "professor";

                return Ok(typeOfUser);
            
        }

        public record class ReqData(string username, string password);

        [HttpPost("/login")]
        public async Task<ActionResult<string>> Login([FromBody] ReqData reqData)
        {
          
            var user = await _userRepository.GetUser(reqData.username);
            if (user == null|| user.Username != reqData.username)
                return BadRequest("User Not Found");

            if (user.Password != reqData.password)
                return BadRequest("Wrong Password");


            string token =  _userRepository.CreateToken(user);
            dynamic flexible = new ExpandoObject();
            var dictionary = (IDictionary<string, Object>)flexible;
            dictionary.Add("access_token", token);
            dictionary.Add("token_expiry", DateTime.Now.AddDays(2));
            dictionary.Add("needTofixDate", false);

            return Ok(dictionary);
        }


  

        [HttpDelete("/removeUser")]
        public async Task<IActionResult> RemoveUser([FromBody] JsonElement requestBody)
        {
            if (!requestBody.TryGetProperty("username", out JsonElement usernameElement))
            {
                return BadRequest("Invalid request body");
            }

            string username = usernameElement.GetString();
            var existingUser = await _context.Users.FirstOrDefaultAsync(user => user.Username == username);
         
            if (existingUser == null)
                return BadRequest("Username don't exists");
            else
            {
                await _userRepository.RemoveUser(username);

                
                return Ok("UserRemovedSuccessfully");
            }
        }
        /// <summary>
        /// /classes
        /// </summary>
        public class UpdateNameRequest
        {
            public string password { get; set; }

            public string NewProperty { get; set; }
        }

        [HttpPut("users/{userId}/changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] UpdateNameRequest request)
        {
            //newPassword 

            var userId = _userRepository.getUserNameByToken();
            if (userId != null)
            {
                var res = await _userRepository.ChangePassword(userId, request.NewProperty, request.password);
                if (res)
                    return Ok(res);
            }

            return BadRequest(new { error = "change failed" });

        }

        [HttpPut("users/{userId}/changeUserName")]
        public async Task<IActionResult> ChangeEmail([FromBody] UpdateNameRequest request)
        {


            var userId = _userRepository.getUserNameByToken();
            if (userId != null)
            {
                var res = await _userRepository.ChangeEmail(userId, request.NewProperty, request.password);
                if (res)
                    return Ok(res);
            }

            return BadRequest(new { error = "change failed" });

        }

        [HttpPut("users/{userId}/changeName")]
        public async Task<IActionResult> ChangeName([FromBody] UpdateNameRequest request)
        {


            Console.WriteLine(request.NewProperty);
            var userId = _userRepository.getUserNameByToken();
            if (userId != null)
            {
                var res = await _userRepository.ChangeName(userId, request.NewProperty, request.password);
                if (res)
                    return Ok(res);
            }

            return BadRequest(new { error = "change failed" });

        }

    }
}
