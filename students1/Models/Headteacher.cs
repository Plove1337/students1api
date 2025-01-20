using System.ComponentModel.DataAnnotations.Schema;

namespace students1.Models
{
    public class Headteacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        [NotMapped]
        public string Role { get; set; } = "Director";
    }
    public class CreateHeadteacher
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class LoginHeadteacher
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
