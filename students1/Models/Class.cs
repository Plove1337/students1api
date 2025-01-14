namespace students1.Models
{
    public class Class
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public List<Student> Students { get; set; } = new List<Student>();
    }
    public class CreateClass
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Student> Students { get; internal set; }
    }
}
