﻿namespace students1.Models
{
    public class Class
    {
        public int Id { get; set; }
        public string ClassName { get; set; }
        public string Description { get; set; }

        public ICollection<Student> Students { get; set; }
    }
}
