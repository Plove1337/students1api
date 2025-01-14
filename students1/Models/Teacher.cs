﻿using System.ComponentModel.DataAnnotations.Schema;

namespace students1.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        [NotMapped]
        public object Role { get; set; } = "Teacher";
        public ICollection<Class> Classes { get; set; } = new List<Class>();
        public ICollection<Student> Students { get; set; } = new List<Student>();


    }
    public class CreateTeacher
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }    }
    public class LoginTeacher
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}