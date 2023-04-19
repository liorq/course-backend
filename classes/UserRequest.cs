namespace JwtWebApi.classes
{
    public class CreateUserRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
        public string BirthDate { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public bool IsStudent { get; set; }
        public string StudentId { get; set; }
        public string? add { get; set; }
        public string? delete { get; set; }
    }
}
