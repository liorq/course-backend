using courses.classes;
using JwtWebApi.classes;
using JwtWebApi.tables;
using Microsoft.EntityFrameworkCore;
using NHibernate.Mapping;
using static JwtWebApi.Controllers.AccountController;

namespace JwtWebApi.Repositories
{
    public interface IStudentsRepository
    {
        public Task<bool> ChangeName(string userName, string newName, string password);
        public  Task<bool> ChangeEmail(string userName, string newUserName, string password);
        public Task<bool> ChangePassword(string userName, string newPassword,string password);
        public  Task<bool> UpdateUserArrivalTime(string userName, Courses course);
        public Task<List<ClassAttendees>> GetUserArrivalTime(string userName);

        public Task<List<ClassAttendees>> GetAllUsersArrivalTime();

        public Task<User> GetUser(string username);

        public Task<List<User>> GetAllStudents();

        public Task<User> CreateUser(CreateUserRequest user);

   

        public Task<bool> RemoveUser(string username);

        public string CreateToken(User user);
        public string? getUserNameByToken();

       

    }
}