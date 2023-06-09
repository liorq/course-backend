﻿namespace JwtWebApi.tables
{
    public class User
    {
        public int Id { get; set; }
        public bool IsLock { get; set; } = false;
        public DateTime Expiry_date { get; set; } = DateTime.Now.AddYears(1);
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? birthDate { get; set; }
        public string? address { get; set; }
    
        public bool? isStudent { get; set; }
        public string? studentId { get; set; }
        public string? phone { get; set; }
        public string? email { get; set; }

    }
}
