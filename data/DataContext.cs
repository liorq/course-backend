using courses.classes;
using JwtWebApi.tables;
using Microsoft.EntityFrameworkCore;

namespace JwtWebApi.data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Courses> Courses { get; set; }
        public DbSet<ClassAttendees> ClassAttendees { get; set; }

    }
}
