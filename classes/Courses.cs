using JwtWebApi.tables;

namespace courses.classes
{
    public class Courses
    {
        public string? Name { get; set; }

        public string? startDate { get; set; }
        public string? endDate { get; set; }
        public string? hours { get; set; }

        public string? date { get; set; }

        public string? days { get; set; }
        public string? CoursesId { get; set; }

        public string? StudentId { get; set; }

        public int? Id { get; set; }
        public string? add { get; set; }
        public string? delete { get; set; }
    }
}
