using System.ComponentModel.DataAnnotations;

namespace User.User.Domain.Entities.Login
{
    public class LoginRequest
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }
    }

    public class Login
    {
        [Required]
        public string? client_id { get; set; }

        [Required]
        public string? scope { get; set; }

        [Required]
        public string? client_secret { get; set; }

        [Required]
        public string? username { get; set; }

        [Required]
        public string? password { get; set; }

        [Required]
        public string? grant_type { get; set; }
    }
}

