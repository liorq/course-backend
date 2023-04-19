using courses.classes;
using JwtWebApi.tables;
using Microsoft.AspNetCore.Mvc;

namespace JwtWebApi.Repositories
{
    public interface ICoursesRepository
    {


        public Task<List<Courses>> GetAllCourses();
        public Task<bool> AddNewCourse(Courses course);

        public Task<bool> RemoveCourse(string CourseName);
        public string? getUserNameByToken();
        public Task<Courses> BuyCourse(Courses course);
        public Task<List<Courses>> GetUserCourses(string userName);

    }
}
