namespace students1.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public int ClassID { get; set; }

        public Class Class { get; set; }
    }
}
